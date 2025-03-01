using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace GilgalInventar.Data
{
    public partial class Entrega
    {
        public Entrega()
        {
            this.Fecha = DateTime.Now;
            this.DespachadoFecha = DateTime.Now;
            this.TransportadoFecha = DateTime.Now;
            this.RecibidoFecha = DateTime.Now;
            this.EntregaFecha = DateTime.Now;
            this.IDTipoMovimiento = 5;
            this.Activo = true;
            this.FlagIn = false;
            this.FlagOut = false;
        }
        public static explicit operator Movimiento(Entrega h)
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
                RecibidoFecha = h.RecibidoFecha,
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
        public long? NoRequisicion { get; set; }
        public string NoOrdenCompra { get; set; }
        public long? IDOrigen { get; set; }
        public long? IDDestino { get; set; }
        public string Conceptos { get; set; }
        public string OtroConcepto { get; set; }
        public DateTime? EntradaFecha { get; set; }
        public string EntradaLugar { get; set; }
        public long? EntradaRecibidoPor { get; set; }
        public string EntradaResponsableRecepcion { get; set; }
        public string EntradaRecibidoFirma { get; set; }
        [DisplayName("Despachado Por")]
        [Required(ErrorMessage = "DespachadoPor Por es requerido.")]
        [Range(1, 999999, ErrorMessage = "DespachadoPor por es requerido.")]
        public long DespachadoPor { get; set; }
        public string DespachadoPorNombre { get; set; }
        public string DespachadoPorCC { get; set; }
        public string DespachadoPorCargo { get; set; }
        [Required(ErrorMessage = "Fecha es requerido.")]
        [DataType(DataType.DateTime)]
        public DateTime DespachadoFecha { get; set; }
        public string DespachadoFirma { get; set; }
        public long? RecibidoPor { get; set; }
        public string RecibidoPorNombre { get; set; }
        public string RecibidoPorCC { get; set; }
        public string RecibidoPorCargo { get; set; }
        public DateTime? RecibidoFecha { get; set; }
        public string RecibidoFirma { get; set; }
        public string TransportadoNombre { get; set; }
        public string TransportadoCC { get; set; }
        public string TransportadoCargo { get; set; }
        public string TransportadoPlaca { get; set; }
        public string TransportadoTipoVehiculo { get; set; }
        public DateTime? TransportadoFecha { get; set; }
        public string Observaciones { get; set; }
        public bool Activo { get; set; }
        public string ArchivoDigital { get; set; }
        [DisplayName("Estado")]
        public string EstadoMovimiento { get; set; } //ABIERTO,DESPACHADO,RECIBIDO,CERRADO
        [DisplayName("Nombre Trabajador o Empresa")]
        [Required(ErrorMessage = "Nombre Trabajador es requerido.")]
        public long EntregaIDPersonal { get; set; }
        public string EntregaNombre { get; set; }
        public string EntregaCargo { get; set; }
        public long? IDAreaFuncional { get; set; }
        public string EntregaCedulaNit { get; set; }
        public string EntregaTelContacto { get; set; }
        public string EntregaRecibidoFirma { get; set; }
        [Required(ErrorMessage = "Fecha es requerido.")]
        [DataType(DataType.DateTime)]
        public DateTime EntregaFecha { get; set; }
        [DisplayName("Centro de Costo")]
        [Required(ErrorMessage = "Centro de Costo es requerido.")]
        public long IDCentroCosto { get; set; }
        [Required(ErrorMessage = "Area es requerido.")]
        [Range(1, 999999, ErrorMessage = "Area es requerido.")]
        public long IDArea { get; set; }
        public bool FlagIn { get; set; }
        public bool FlagOut { get; set; }
        public long? IDMovimientoRel { get; set; }
    }
}
