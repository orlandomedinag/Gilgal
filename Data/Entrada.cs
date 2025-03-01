using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GilgalInventar.Data
{
    public class Entrada
    {
        public Entrada()
        {
            this.Fecha = DateTime.Now;
            this.DespachadoFecha = DateTime.Now;
            this.TransportadoFecha = DateTime.Now;
            this.RecibidoFecha = DateTime.Now;
            this.EntradaFecha = DateTime.Now;
            this.IDTipoMovimiento = 2;
            this.Activo = true;
            this.FlagIn = false;
            this.FlagOut = true;
        }
        public static explicit operator Movimiento(Entrada h)
        {
            return new Movimiento()
            {
                IDMovimiento = h.IDMovimiento,
                Fecha = h.Fecha,
                IDTipoMovimiento = h.IDTipoMovimiento,
                Consecutivo = h.Consecutivo,
                Prefijo = h.Prefijo,
                NoRequisicion = h.NoRequisicion,
                NoOrdenCompra = h.NoOrdenCompra,
                IDOrigen = h.IDOrigen,
                IDDestino = h.IDDestino,
                Conceptos = h.Conceptos,
                OtroConcepto = h.OtroConcepto,
                EntradaFecha = h.EntradaFecha,
                EntradaLugar = h.EntradaLugar,
                EntradaRecibidoPor = h.EntradaRecibidoPor,
                EntradaResponsableRecepcion = h.EntradaResponsableRecepcion,
                EntradaRecibidoFirma = h.EntradaRecibidoFirma,
                DespachadoPor = h.DespachadoPor,
                DespachadoPorNombre = h.DespachadoPorNombre,
                DespachadoPorCC = h.DespachadoPorCC,
                DespachadoPorCargo = h.DespachadoPorCargo,
                DespachadoFecha = h.DespachadoFecha,
                DespachadoFirma = h.DespachadoFirma,
                RecibidoPor = h.RecibidoPor,
                RecibidoPorNombre = h.RecibidoPorNombre,
                RecibidoPorCargo = h.RecibidoPorCargo,
                RecibidoPorCC = h.RecibidoPorCC,
                RecibidoFecha = (DateTime)h.RecibidoFecha,
                RecibidoFirma = h.RecibidoFirma,
                TransportadoNombre = h.TransportadoNombre,
                TransportadoCC = h.TransportadoCC,
                TransportadoCargo = h.TransportadoCargo,
                TransportadoPlaca = h.TransportadoPlaca,
                TransportadoTipoVehiculo = h.TransportadoTipoVehiculo,
                TransportadoFecha = h.TransportadoFecha,
                Observaciones = h.Observaciones,
                ArchivoDigital = h.ArchivoDigital,
                EstadoMovimiento = h.EstadoMovimiento,
                EntregaIDPersonal = h.EntregaIDPersonal,
                EntregaNombre = h.EntregaNombre,
                EntregaCargo = h.EntregaCargo,
                IDAreaFuncional = h.IDAreaFuncional,
                EntregaCedulaNit = h.EntregaCedulaNit,
                EntregaTelContacto = h.EntregaTelContacto,
                EntregaRecibidoFirma = h.EntregaRecibidoFirma,
                EntregaFecha = h.EntregaFecha,
                IDCentroCosto = h.IDCentroCosto,
                IDArea = h.IDArea,
                Activo = h.Activo,
                FlagIn = h.FlagIn,
                FlagOut = h.FlagOut,
                IDMovimientoRel = h.IDMovimientoRel
            };
        }

        [Key]
        [Editable(false)]
        public long IDMovimiento { get; set; }
        [DisplayName("Fecha")]
        [Required(ErrorMessage = "Fecha es requerido.")]
        [DataType(DataType.DateTime)]
        public DateTime Fecha { get; set; }
        [Required(ErrorMessage = "Tipo Movimiento es requerido.")]
        public long IDTipoMovimiento { get; set; }
        public long? Consecutivo { get; set; }
        public string Prefijo { get; set; }
        [DisplayName("No. requisición")]
        [Required(ErrorMessage = "No Requisicion es requerido.")]
        public long NoRequisicion { get; set; }
        [DisplayName("No. de Orden")]
        [Required(ErrorMessage = "Orden de compra es requerido.")]
        [StringLength(20)]
        public string NoOrdenCompra { get; set; }
        [DisplayName("Origen")]
        [Required(ErrorMessage = "Origen es requerido.")]
        [Range(1, 999999, ErrorMessage = "Origen es requerido.")]
        public long IDOrigen { get; set; }
        [DisplayName("Destino")]
        [Required(ErrorMessage = "Destino es requerido.")]
        [Range(1, 999999, ErrorMessage = "Destino es requerido.")]
        public long IDDestino { get; set; }
        [DisplayName("Conceptos")]
        public string Conceptos { get; set; }
        [DisplayName("Otro")]
        public string OtroConcepto { get; set; }
        public DateTime? EntradaFecha { get; set; }
        [DisplayName("Responsable Recepción")]
        public long? EntradaRecibidoPor { get; set; }
        public string EntradaLugar { get; set; }
        public string EntradaResponsableRecepcion { get; set; }
        public string EntradaRecibidoFirma { get; set; }
        [DisplayName("Despachado Por")]
        public long? DespachadoPor { get; set; }
        [Required(ErrorMessage = "Despachado por es requerido.")]
        public string DespachadoPorNombre { get; set; }
        [Required(ErrorMessage = "CC es requerido.")]
        public string DespachadoPorCC { get; set; }
        [Required(ErrorMessage = "Cargo es requerido.")]
        public string DespachadoPorCargo { get; set; }
        [DisplayName("Fecha despacho")]
        [Required(ErrorMessage = "Fecha despachado es requerido.")]
        [DataType(DataType.Date, ErrorMessage = "No tiene el formato correcto")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime DespachadoFecha { get; set; }
        public string DespachadoFirma { get; set; }
        [DisplayName("Recibido Por")]
        [Required(ErrorMessage = "Recibido por es requerido.")]
        [Range(1, 999999, ErrorMessage = "Recibido por es requerido.")]
        public long RecibidoPor { get; set; }
        public string RecibidoPorNombre { get; set; }
        public string RecibidoPorCC { get; set; }
        public string RecibidoPorCargo { get; set; }
        [DisplayName("Fecha recibido")]
        [Required(ErrorMessage = "Fecha recibido es requerido.")]
        [DataType(DataType.Date, ErrorMessage = "No tiene el formato correcto")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime RecibidoFecha { get; set; }
        [DisplayName("Firma Recibido")]
        public string RecibidoFirma { get; set; }
        [Required(ErrorMessage = "Transportado por es requerido.")]
        [DisplayName("Transportado por")]
        public string TransportadoNombre { get; set; }
        [DisplayName("CC")]
        [Required(ErrorMessage = "CC por es requerido.")]
        public string TransportadoCC { get; set; }
        [DisplayName("Cargo")]
        [Required(ErrorMessage = "Cargo por es requerido.")]
        public string TransportadoCargo { get; set; }
        [DisplayName("Placa")]
        [Required(ErrorMessage = "Placa por es requerido.")]
        public string TransportadoPlaca { get; set; }
        [DisplayName("Tipo Vehículo")]
        [Required(ErrorMessage = "Tipo Vehículo por es requerido.")]
        public string TransportadoTipoVehiculo { get; set; }
        [Required(ErrorMessage = "Transportado por es requerido.")]
        public DateTime? TransportadoFecha { get; set; }
        public string Observaciones { get; set; }
        public bool Activo { get; set; }
        public string ArchivoDigital { get; set; }
        [DisplayName("Estado")]
        public string EstadoMovimiento { get; set; } //ABIERTO,DESPACHADO,RECIBIDO,CERRADO
        public long? EntregaIDPersonal { get; set; }
        public string EntregaNombre { get; set; }
        public string EntregaCargo { get; set; }
        public long? IDAreaFuncional { get; set; }
        public string EntregaCedulaNit { get; set; }
        public string EntregaTelContacto { get; set; }
        public string EntregaRecibidoFirma { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? EntregaFecha { get; set; }
        public long? IDCentroCosto { get; set; }
        [Required(ErrorMessage = "Area es requerido.")]
        [Range(1, 999999, ErrorMessage = "Area es requerido.")]
        public long IDArea { get; set; }
        public bool FlagIn { get; set; }
        public bool FlagOut { get; set; }
        public long? IDMovimientoRel { get; set; }
    }
}
