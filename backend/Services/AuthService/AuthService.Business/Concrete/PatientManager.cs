using AuthService.Business.Abstract;
using AuthService.Business.External.Dtos;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Business.Concrete
{
    public class PatientManager : IPatientService
    {
        private readonly HttpClient _httpClient;

        public PatientManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreatePatient(CreatePatientDto createPatientDto)
        {
            var response = await _httpClient.PostAsJsonAsync("", createPatientDto);
            response.EnsureSuccessStatusCode();
        }
    }
}
