using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos
{
    public class RatingWithUserDto
    {
        public RatingApplicationDto RatingApplication { get; set; }
        public AccountImageDto Account { get; set; }
    }
}
