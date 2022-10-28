using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CAdministracionPuestoL
    {
        #region Variables

        private SIRHEntities contexto;

        #endregion

        #region Constructor

        public CAdministracionPuestoL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Metodos
        //FuncionarioL  Y PuestoL, los meto en AdministracionPuestoL, para buscar por codPuesto en DATOS.       

        //Busca el puesto, por medio del codigo de puesto del empleado         
        private CPuestoDTO CargarPuestoParam(string codPuesto)
        {
            //En esta variable vamos a almacenar los datos que provienen de la BD
            CPuestoDTO respuesta = new CPuestoDTO();

            Puesto temp = new Puesto();
            CPuestoD intermedio = new CPuestoD(contexto);
            temp = intermedio.CargarPuestoParam(codPuesto);

            respuesta.CodPuesto = temp.CodPuesto;
            respuesta.EstadoPuesto = new CEstadoPuestoDTO { IdEntidad = temp.EstadoPuesto.PK_EstadoPuesto };
            respuesta.UbicacionAdministrativa = new CUbicacionAdministrativaDTO { IdEntidad = temp.UbicacionAdministrativa.PK_UbicacionAdministrativa };

            return respuesta;
        }
        
        private CFuncionarioDTO BuscarFuncionarioDetallePuesto(string codPuesto)
        {
            CFuncionarioDTO respuesta = new CFuncionarioDTO();
            Funcionario temp = new Funcionario();


            CFuncionarioD intermdio = new CFuncionarioD(contexto);

            temp = intermdio.BuscarFuncionarioDetallePuesto(codPuesto, 0, 0, 0).FirstOrDefault();

            respuesta.Cedula = temp.IdCedulaFuncionario;
            respuesta.Nombre = temp.NomFuncionario;
            respuesta.PrimerApellido = temp.NomPrimerApellido;
            respuesta.SegundoApellido = temp.NomSegundoApellido;
            respuesta.FechaNacimiento = Convert.ToDateTime(temp.FecNacimiento);
            //respuesta.Sexo = temp.IndSexo;
            //respuesta.EstadoFuncionario = temp.EstadoFuncionario.PK_EstadoFuncionario;

            return respuesta;
        }

        //Se insertó en ICPuestoService y CPuestoService el 31/01/2017
        public List<CBaseDTO> BuscarDatosPuesto(string codPuesto)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            CPuestoDTO puestotemp = new CPuestoDTO();
            CFuncionarioDTO functemp = new CFuncionarioDTO();

            puestotemp = this.CargarPuestoParam(codPuesto);
            functemp = this.BuscarFuncionarioDetallePuesto(codPuesto);

            respuesta.Add(puestotemp);
            respuesta.Add(functemp);

            return respuesta;
        }

        #endregion
    }
}
    

