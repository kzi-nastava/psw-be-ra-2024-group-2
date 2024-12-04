using AutoMapper;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payment.API.Dtos;
using Explorer.Payment.API.Public.Author;
using Explorer.Payment.Core.Domain;
using Explorer.Payment.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.API.Internal.Administration;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.Core.UseCases.Author
{
    public class TourSaleService : CrudService<TourSaleDto, TourSale>, ITourSaleService
    {
        public ICrudRepository<TourSale> _crudRepository;
        public ITourSaleRepository _tourSaleRepository;
        private readonly ITourService_Internal _tourServiceInternal;
        private readonly IMapper _mapper;
        public TourSaleService(ICrudRepository<TourSale> crudRepository, ITourSaleRepository tourSaleRepository, ITourService_Internal tourServiceInternal, IMapper mapper) : base(crudRepository, mapper)
        {
            _crudRepository = crudRepository;
            _tourSaleRepository = tourSaleRepository;
            _mapper = mapper;
            _tourServiceInternal = tourServiceInternal;
        }

        public Result<List<TourSaleDto>> GetAll()
        {
            var tourSales = CrudRepository.GetPaged(0, 0);

            var tourSaleDtos = tourSales.Results.Select(tourSale => new TourSaleDto
            {
                Id = tourSale.Id,
                Name = tourSale.Name,
                StartDate = tourSale.StartDate,
                EndDate = tourSale.EndDate,
                UserId = tourSale.UserId,
                DiscountPercentage = tourSale.DiscountPercentage,
                Tours = tourSale.Tours.Select(tour =>
                {
                    var tourDetails = _tourServiceInternal.GetById(tour.TourId);
                    return new TourDtoPayment
                    {
                        Id = tourDetails.Value.Id,
                        UserId = tourDetails.Value.UserId,
                        Name = tourDetails.Value.Name,
                        Description = tourDetails.Value.Description,
                        Price = tourDetails.Value.Price,
                        Difficulty = tourDetails.Value.Difficulty,
                        Tag = tourDetails.Value.Tag,
                        Status = tourDetails.Value.Status,
                        Equipment = tourDetails.Value.Equipment,
                        Checkpoints = tourDetails.Value.Checkpoints,
                        Prices = new List<TourPricesDto>
                {
                    new TourPricesDto
                    {
                        TourId = tourDetails.Value.Id,
                        OldPrice = tourDetails.Value.Price,
                        NewPrice = tourDetails.Value.Price * (1 - tourSale.DiscountPercentage / 100.0)
                    }
                }
                    };
                }).ToList(),
            }).ToList();


            var expiredTours = tourSaleDtos.Where(tourSale => tourSale.EndDate < DateTime.Now).ToList();

            foreach (var expiredTour in expiredTours)
            {
                try
                {
                    Delete(expiredTour.Id);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to delete tour with ID {expiredTour.Id}: {ex.Message}");
                }
            }

            var activeTours = tourSaleDtos.Where(tourSale => tourSale.EndDate >= DateTime.Now).ToList();

            return Result.Ok(activeTours);
        }

        public Result<TourSaleDto> Create(TourSaleDto tourSaleDto, int userId)
        {
            var tourSales = GetAll();
            var existingTours = tourSales.Value
                .SelectMany(tourSale => tourSale.Tours)
                .ToList();

            if ((tourSaleDto.StartDate.AddDays(15)) < tourSaleDto.EndDate)
            {
                return Result.Fail<TourSaleDto>($"Tour with name {tourSaleDto.Name} cannot last longer than 15 days.");
            }

            foreach (var newTour in tourSaleDto.Tours)
            {
                if (existingTours.Any(existingTour => existingTour.Id == newTour.Id))
                {
                    return Result.Fail<TourSaleDto>($"Tour with ID {newTour.Id} is already in sale.");
                }
            }
            try
            {
                foreach (var tour in tourSaleDto.Tours)
                {
                    var tourCheck = _tourServiceInternal.GetById(tour.Id);
                    if (tourCheck == null)
                    {
                        return Result.Fail<TourSaleDto>($"Tour with ID {tour.Id} does not exist.");
                    }
                }

                var tourSale = _mapper.Map<TourSale>(tourSaleDto);
                tourSale.UserId = userId;

                var createdTourSale = _crudRepository.Create(tourSale);

                return Result.Ok(_mapper.Map<TourSaleDto>(createdTourSale));
            }
            catch (Exception ex)
            {
                return Result.Fail<TourSaleDto>("An error occurred while creating the tour sale: " + ex.Message);
            }
        }

        public Result<TourSaleDto> Update(int id, TourSaleDto tourSaleDto)
        {
            try
            {
                var tourSale = _crudRepository.Get(id);
                if (tourSale == null)
                {
                    return Result.Fail<TourSaleDto>("Tour sale not found.");
                }

                foreach (var tour in tourSaleDto.Tours)
                {
                    var tourCheck = _tourServiceInternal.GetById(tour.Id);
                    if (tourCheck == null)
                    {
                        return Result.Fail<TourSaleDto>($"Tour with ID {tour.Id} does not exist.");
                    }
                }

                _mapper.Map(tourSaleDto, tourSale);
                var updatedTourSale = _crudRepository.Update(tourSale);
                return Result.Ok(_mapper.Map<TourSaleDto>(updatedTourSale));
            }
            catch (Exception ex)
            {
                return Result.Fail<TourSaleDto>("An error occurred while updating the tour sale: " + ex.Message);
            }
        }

        public Result<bool> Delete(long id)
        {
            try
            {
                var tourSale = _crudRepository.Get(id);
                if (tourSale == null)
                {
                    return Result.Fail<bool>("Tour sale not found.");
                }

                _crudRepository.Delete(tourSale.Id);
                return Result.Ok(true);
            }
            catch (Exception ex)
            {
                return Result.Fail<bool>("An error occurred while deleting the tour sale: " + ex.Message);
            }
        }
    }
}
