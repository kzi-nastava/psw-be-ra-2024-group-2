using AutoMapper;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Internal;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.UseCases
{
    public class FAQService : CrudService<FAQDto, FAQ>, IFAQService
    {
        private readonly ICrudRepository<FAQ> _repository;
        private readonly ITransactionRepository _transactionRepository;

        private readonly IProfileService_Internal _personService;

        public FAQService(ICrudRepository<FAQ> repository, ITransactionRepository transactionRepository, IMapper mapper, IProfileService_Internal personService) : base(repository, mapper)
        {
            _repository = repository;
            _transactionRepository = transactionRepository;
            _personService = personService;
        }

        public Result<PagedResult<FAQDto>> GetPaged(int page, int pageSize)
        {
            List<FAQDto> faqDtos = new List<FAQDto>();
            var allFAQs = _repository.GetPaged(page, pageSize);

            List<FAQ> faqs = new List<FAQ>();


            foreach(var f in allFAQs.Results)
            {
                faqs.Add(f);
            }
            
            foreach (var fa in faqs)
                faqDtos.Add(MapToDto(fa));

            return new PagedResult<FAQDto>(faqDtos, faqDtos.Count());
        }
        public Result<FAQDto> Create(long userId, FAQDto faq)
        {
            var person = _personService.GetAccount(userId);
            if (person.Value.Role != UserRole.Administrator)
            {
                return Result.Fail(FailureCode.Conflict).WithError("Only admins can create FAQ entries.");
            }

            if (string.IsNullOrWhiteSpace(faq.Question) || string.IsNullOrWhiteSpace(faq.Answer))
            {
                return Result.Fail(FailureCode.Conflict).WithError("Question and Answer cannot be empty.");
            }

            try
            {
                var newFAQ = MapToDomain(faq);
                var results = _repository.Create(newFAQ);
                return MapToDto(results);
            }
            catch (Exception e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
        }

        public Result<FAQDto> Update(int faqId, long userId, FAQDto faq)
        {
            var person = _personService.GetAccount(userId);
            if (person.Value.Role != UserRole.Administrator)
            {
                return Result.Fail(FailureCode.Conflict).WithError("Only admins can create FAQ entries.");
            }

            if (string.IsNullOrWhiteSpace(faq.Question) || string.IsNullOrWhiteSpace(faq.Answer))
            {
                return Result.Fail(FailureCode.Conflict).WithError("Question and Answer cannot be empty.");
            }
            try
            {
                var existingFAQ = _repository.Get(faqId);
                if (existingFAQ == null)
                {
                    return Result.Fail(FailureCode.NotFound).WithError("FAQ not found.");
                }

                existingFAQ.UpdateQuestion(faq.Question);
                existingFAQ.UpdateAnswer(faq.Answer);
                existingFAQ.UpdateLastUpdatedDate(faq.LastUpdatedDate);

                var updatedEntity = _repository.Update(existingFAQ);

                return MapToDto(updatedEntity);
            }
            catch (Exception e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
        }
    }
}
