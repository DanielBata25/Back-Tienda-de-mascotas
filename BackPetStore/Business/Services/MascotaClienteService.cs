using Business.Core;
using Data.Interfaces;
using Entity.DTO.Create;
using Entity.DTOs;
using Entity.Model;
using Mapster;
using Microsoft.Extensions.Logging;
using static Dapper.SqlMapper;

namespace Business.Services
{
    public class MascotaClienteService : ServiceBase<MascotaClienteDto, MascotaClienteCreateDto, MascotaCliente>
    {
        
        public MascotaClienteService(IMascotaClienteRepository repository, ILogger<MascotaClienteService> logger)
            : base(repository, logger) { 


        
        }

        public override async Task<IEnumerable<MascotaClienteDto>> GetAllAsync()
        {
            try
            {
                var entities = await _repository.GetAllAsync();
                return entities.Adapt<IEnumerable<MascotaClienteDto>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los registros de {Entity}", typeof(MascotaCliente).Name);
                throw;
            }
        }
    }
}
