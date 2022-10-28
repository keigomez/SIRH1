using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SIRH.DTO;
using System.Web.Mvc;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Collections.Generic;

namespace SIRH.Web.ViewModels
{
    public class FormularioPagoFeriadoVM
    {
        //Pago Extraordinario
        public CPagoExtraordinarioDTO PagoExtraordinario { get; set; }

        //Pago feriado
        public CPagoFeriadoDTO PagoFeriados { get; set; }

        //Funcionario
        public CFuncionarioDTO Funcionario { get; set; }
        public CPuestoDTO Puesto { get; set; }
        public CDetallePuestoDTO DetallePuesto { get; set; }
        public CUbicacionAdministrativaDTO UbicacionAdministrativa { get; set; }
        public CDesgloseSalarialDTO DesgloseSalarial { get; set; }
        public CDetalleContratacionDTO DetalleContratacion { get; set; }

        //Estado trámite
        public CEstadoTramiteDTO EstadoTramite { get; set; }
        public int TipoTramite { get; set; }

        //Deducciones
        public List<CDeduccionEfectuadaDTO> DeduccionesEfectuadas { get; set; }
        public List<CCatalogoDeduccionDTO> CatalogoDeduccion { get; set; }
        public List<CCatalogoDeduccionDTO> DeduccionesObrero { get; set; }
        public List<CCatalogoDeduccionDTO> DeduccionesPatronal { get; set; }
        public List<CDeduccionEfectuadaDTO> DeduccionesObreroEfectuada { get; set; }
        public List<CDeduccionEfectuadaDTO> DeduccionesPatronalEfectuada { get; set; }
        public CDeduccionEfectuadaDTO DeduccionesEfectuadasObreraAuxiliar { get; set; }
        public CDeduccionEfectuadaDTO DeduccionesEfectuadasPatronalAuxiliar { get; set; }
        public CCatalogoDeduccionDTO CatalogoDeduccionAuxiliar { get; set; }
        public CCatalogoDeduccionDTO DeduccionesObreroAuxiliar { get; set; }
        public CCatalogoDeduccionDTO DeduccionesPatronalAuxiliar { get; set; }

        //Días
        public CCatalogoDiaDTO CatalogoDiaAuxiliar { get; set; }
        public List<CCatalogoDiaDTO> CatalogoDiaFeriado { get; set; }
        public List<CCatalogoDiaDTO> CatalogoDiaAsueto { get; set; }
        public List<CDiaPagadoDTO> DiasPagados { get; set; }
        public CDiaPagadoDTO DiaPagadoAuxiliar { get; set; }

        public CCatalogoRemuneracionDTO SalEscolar { get; set; }
        public CRemuneracionEfectuadaPFDTO SalEscolarEfectuado { get; set; }

        //Error
        public CErrorDTO Error { get; set; }

        [DisplayName("Observaciones")]
        public string Observaciones { get; set; }

        //Select list
        public SelectList TipoNombramiento { get; set; }
        [DisplayName("Tipo nombramiento")]
        public int NombramientoSeleccionado { get; set; }

        [DisplayName("Subtotal")]
        public string SubtotalDias { get; set; }

        [DisplayName("Salario escolar")]
        public string porcentajeSalarioEscolar { get; set; }

        [DisplayName("Diferencia Líquida")]
        public decimal DiferenciaLiquida { get; set; }

        [DisplayName("Aguinaldo proporcional")]
        public decimal AguinaldoProporcional { get; set; }

        [DisplayName("Diferencia del periodo")]
        public decimal DiferenciaPeriodo { get; set; }

        [DisplayName("Salario escolar")]
        public decimal SalarioEscolar { get; set; }

        [DisplayName("Subtotal")]
        public decimal SubtotalDiferencias { get; set; }

        [DisplayName("Total")]
        public decimal MontoTotal { get; set; }

        //Search asueto

        public int TotalAsuetos { get; set; }
        public int TotalPaginas { get; set; }
        public int PaginaActual { get; set; }
        public SelectList Canton { get; set; }
        [DisplayName("Cantón")]
        public string CantonSeleccionado { get; set; }
        public string NombreSearch { get; set; }
    }
}