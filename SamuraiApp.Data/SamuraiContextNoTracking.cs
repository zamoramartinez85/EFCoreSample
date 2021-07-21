using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamuraiApp.Data
{
    public class SamuraiContextNoTracking : SamuraiContext
    {
        public SamuraiContextNoTracking()
        {
            //Hacemos esto para evitar que el contexto realice trazas de los objetos
            //Esto se emplea para optimizar recursos de EF Core y se emplea en 
            //escenario desconectados.
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
    }
}
