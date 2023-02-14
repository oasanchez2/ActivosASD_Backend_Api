using GrupoASD.GestionActivos.Api.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrupoASD.GestionActivos.Api.Servicios
{
    public interface ILogsErrorReposotorio
    {
        void Insert(LogsError logsError);
        Task<long> InsertAndSaveAsync(Exception ex);
        Task SaveAsync();
    }
    /// <summary>
    /// Repositorio con los metodos de acceso a los datos del log de errores de la aplicación
    /// </summary>
    public class LogsErrorReposotorio : ILogsErrorReposotorio
    {
        private readonly ActivosASDContext _context;
        private readonly ILogger<LogsErrorReposotorio> _logger;

        public LogsErrorReposotorio(ActivosASDContext context,
                                   ILogger<LogsErrorReposotorio> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Inserta un log listo para ser enviado a la base de datos
        /// </summary>
        /// <param name="transactions"></param>
        /// <returns></returns>
        public void Insert(LogsError logsError)
        {
            _context.LogsError.Add(logsError);
        }

        /// <summary>
        /// Inserta y guarda el logs en la base de datos
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public async Task<long> InsertAndSaveAsync(Exception ex)
        {
            try
            {
                LogsError logsError = new LogsError
                {
                    OrignError = ex.Source,
                    MethodError = ex.TargetSite == null ? "" : ex.TargetSite.DeclaringType == null ? "" : ex.TargetSite.DeclaringType.Name,
                    MessageError = ex.Message,
                    StackTrace = ex.StackTrace == null ? "" : ex.StackTrace,
                    InnerException = ex.InnerException == null ? "" : ex.InnerException.Message,
                    DayError = DateTime.Now
                };
                Insert(logsError);
                await SaveAsync();
                return logsError.IdLogError;
            }
            catch (Exception exep)
            {
                _logger.LogCritical(0, "Exception. {0}", exep.Message);
                return 0;
            }

        }

        /// <summary>
        /// Guarda en base de datos
        /// </summary>
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
