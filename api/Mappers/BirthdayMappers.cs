using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using api.Dtos.Birthday;

namespace api.Mappers
{
    public static class BirthdayMappers
    {
        public static BirthdayDto ToBirthdayDto(this Birthday birthdayModel)
        {
            return new BirthdayDto{
                Id=birthdayModel.Id,
                Name=birthdayModel.Name,
                Date=birthdayModel.Date,
                FileName=birthdayModel.FileName,
                FileData=birthdayModel.FileData
            };
        }

        public static Birthday ToBirthdayFromCreateDto(this CreateBirthdayRequestDto birthdayDto){
            return new Birthday{
                Name=birthdayDto.Name,
                Date=birthdayDto.Date,
                FileName=birthdayDto.FileName,
                FileData=birthdayDto.FileData
            };
        }
    }
}