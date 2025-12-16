using AuthService.Business.External.Dtos;
using Core.Utilities.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Business.Abstract
{
    public interface IPatientService
    {
        Task CreatePatient(CreatePatientDto createPatientDto);
    }
}
