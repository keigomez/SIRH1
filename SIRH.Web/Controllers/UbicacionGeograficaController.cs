using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIRH.Web.ViewModels;
using SIRH.DTO;

namespace SIRH.Web.Controllers
{
    public class UbicacionGeograficaController : Controller
    {
        //
        // GET: /UbicacionGeografica/GetProvincias

        public ActionResult GetProvincias(SeleccionProvinciaVM model)
        {
            var provincias = model.EntidadesProvincia
                .Select(Q => new SelectListItem
                {
                    Value = ((CProvinciaDTO)Q).IdEntidad.ToString(),
                    Text = ((CProvinciaDTO)Q).NomProvincia
                });

            model.Provincias = new SelectList(provincias, "Value", "Text");

            return View(model);
        }

        //
        // GET: /UbicacionGeografica/GetCantones

        public ActionResult GetCantones(int? provincia, SeleccionCantonVM model)
        {
            if (provincia != null)
            {
                int prov = Convert.ToInt32(provincia);
                var cantones = model.EntidadesCanton
                    .Where(Q => Q.Provincia.IdEntidad == prov)
                    .Select(Q => new SelectListItem
                    {
                        Value = ((CCantonDTO)Q).IdEntidad.ToString(),
                        Text = ((CCantonDTO)Q).NomCanton
                    });

                model.Cantones = new SelectList(cantones, "Value", "Text");
            }

            return View(model);
        }

        //
        // GET: /UbicacionGeografica/GetDistritos

        public ActionResult GetDistritos(int? canton, SeleccionDistritoVM model)
        {
            if (canton != null)
            {
                int cant = Convert.ToInt32(canton);
                var distritos = model.EntidadesDistrito
                    .Where(Q => Q.Canton.IdEntidad == cant)
                    .Select(Q => new SelectListItem
                    {
                        Value = ((CDistritoDTO)Q).IdEntidad.ToString(),
                        Text = ((CDistritoDTO)Q).NomDistrito
                    });

                model.Distritos = new SelectList(distritos, "Value", "Text");
            }

            return View(model);
        }
    }
}
