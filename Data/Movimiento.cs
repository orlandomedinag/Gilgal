using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


#nullable disable

namespace GilgalInventar.Data
{
    public partial class Movimiento
    {
        public Movimiento()
        {
            this.Fecha = DateTime.Now;
            this.DespachadoFecha = DateTime.Now;
            this.TransportadoFecha = DateTime.Now;
            this.RecibidoFecha = DateTime.Now;
            this.EntradaFecha = DateTime.Now;
            this.Activo = true;
        }

        public static explicit operator Entrada(Movimiento h)
        {
            return new Entrada() {
                IDMovimiento = h.IDMovimiento,
                Fecha = h.Fecha,
                IDTipoMovimiento = h.IDTipoMovimiento,
                Consecutivo = h.Consecutivo,
                Prefijo = h.Prefijo,
                NoRequisicion = (long)h.NoRequisicion,
                NoOrdenCompra = h.NoOrdenCompra,
                IDOrigen = (long)h.IDOrigen,
                IDDestino = (long)h.IDDestino,
                Conceptos = h.Conceptos,
                OtroConcepto = h.OtroConcepto,
                EntradaFecha = h.EntradaFecha,
                EntradaRecibidoPor = h.EntradaRecibidoPor,
                EntradaLugar = h.EntradaLugar,
                EntradaResponsableRecepcion = h.EntradaResponsableRecepcion,
                EntradaRecibidoFirma = h.EntradaRecibidoFirma,
                DespachadoPor = h.DespachadoPor,
                DespachadoPorNombre = h.DespachadoPorNombre,
                DespachadoPorCC = h.DespachadoPorCC,
                DespachadoPorCargo = h.DespachadoPorCargo,
                DespachadoFecha = (DateTime)h.DespachadoFecha,
                DespachadoFirma = h.DespachadoFirma,
                RecibidoPor = (long)h.RecibidoPor,
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

        public static explicit operator Salida(Movimiento h)
        {
            return new Salida()
            {
                IDMovimiento = h.IDMovimiento,
                Fecha = h.Fecha,
                IDTipoMovimiento = h.IDTipoMovimiento,
                Consecutivo = h.Consecutivo,
                Prefijo = h.Prefijo,
                NoRequisicion = (long)h.NoRequisicion,
                NoOrdenCompra = h.NoOrdenCompra,
                IDOrigen = (long)h.IDOrigen,
                IDDestino = (long)h.IDDestino,
                Conceptos = h.Conceptos,
                OtroConcepto = h.OtroConcepto,
                EntradaFecha = h.EntradaFecha,
                EntradaLugar = h.EntradaLugar,
                EntradaRecibidoPor = h.EntradaRecibidoPor,
                EntradaResponsableRecepcion = h.EntradaResponsableRecepcion,
                EntradaRecibidoFirma = h.EntradaRecibidoFirma,
                DespachadoPor = (long)h.DespachadoPor,
                DespachadoPorNombre = h.DespachadoPorNombre,
                DespachadoPorCC = h.DespachadoPorCC,
                DespachadoPorCargo = h.DespachadoPorCargo,
                DespachadoFecha = (DateTime)h.DespachadoFecha,
                DespachadoFirma = h.DespachadoFirma,
                RecibidoPor = (long)h.RecibidoPor,
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
                TransportadoFecha = (DateTime)h.TransportadoFecha,
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
        
        public static explicit operator Entrega(Movimiento h)
        {
            return new Entrega()
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
                DespachadoPor = (long)h.DespachadoPor,
                DespachadoPorNombre = h.DespachadoPorNombre,
                DespachadoPorCC = h.DespachadoPorCC,
                DespachadoPorCargo = h.DespachadoPorCargo,
                DespachadoFecha = (DateTime)h.DespachadoFecha,
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
                EntregaIDPersonal = (long)h.EntregaIDPersonal,
                EntregaNombre = h.EntregaNombre,
                EntregaCargo = h.EntregaCargo,
                IDAreaFuncional = h.IDAreaFuncional,
                EntregaCedulaNit = h.EntregaCedulaNit,
                EntregaTelContacto = h.EntregaTelContacto,
                EntregaRecibidoFirma = h.EntregaRecibidoFirma,
                IDCentroCosto = (long)h.IDCentroCosto,
                IDArea = h.IDArea,
                Activo = h.Activo,
                FlagIn = h.FlagIn,
                FlagOut = h.FlagOut,
                IDMovimientoRel = h.IDMovimientoRel
            };
        }

        public static explicit operator EntregaEPP(Movimiento h)
        {
            return new EntregaEPP()
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
                DespachadoPor = (long)h.DespachadoPor,
                DespachadoPorNombre = h.DespachadoPorNombre,
                DespachadoPorCC = h.DespachadoPorCC,
                DespachadoPorCargo = h.DespachadoPorCargo,
                DespachadoFecha = (DateTime)h.DespachadoFecha,
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
                EntregaIDPersonal = (long)h.EntregaIDPersonal,
                EntregaNombre = h.EntregaNombre,
                EntregaCargo = h.EntregaCargo,
                IDAreaFuncional = (long)h.IDAreaFuncional,
                EntregaCedulaNit = h.EntregaCedulaNit,
                EntregaTelContacto = h.EntregaTelContacto,
                EntregaRecibidoFirma = h.EntregaRecibidoFirma,
                IDCentroCosto = (long)h.IDCentroCosto,
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
        public long? NoRequisicion { get; set; }
        [DisplayName("No. de Orden")]
        [StringLength(20)]
        public string NoOrdenCompra { get; set; }
        [DisplayName("Origen")]
        public long? IDOrigen { get; set; }
        [DisplayName("Destino")]
        public long? IDDestino { get; set; }
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
        public string DespachadoPorNombre { get; set; }
        public string DespachadoPorCC { get; set; }
        public string DespachadoPorCargo { get; set; }
        [DisplayName("Fecha despacho")]
        public DateTime? DespachadoFecha { get; set; }
        public string DespachadoFirma { get; set; }
        [DisplayName("Recibido Por")]
        public long? RecibidoPor { get; set; }
        public string RecibidoPorNombre { get; set; }
        public string RecibidoPorCC { get; set; }
        public string RecibidoPorCargo { get; set; }
        [DisplayName("Fecha recibido")]
        public DateTime? RecibidoFecha { get; set; }
        [DisplayName("Firma Recibido")]
        public string RecibidoFirma { get; set; }
        [DisplayName("Transportado por")]
        public string TransportadoNombre { get; set; }
        [DisplayName("CC")]
        public string TransportadoCC { get; set; }
        public string TransportadoCargo { get; set; }
        [DisplayName("Placa")]
        public string TransportadoPlaca { get; set; }
        public string TransportadoTipoVehiculo { get; set; }
        [Required(ErrorMessage = "Fecha de trasporte es requerido.")]
        [DisplayName("Fecha de trasporte")]
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
        public long IDArea { get; set; }
        public bool FlagIn { get; set; }
        public bool FlagOut { get; set; }
        public long? IDMovimientoRel { get; set; }
    }
}
