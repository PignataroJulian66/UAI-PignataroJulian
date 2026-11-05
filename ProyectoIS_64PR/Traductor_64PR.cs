using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoIS_64PR
{
    public static class Traductor_64PR
    {
        public static void Traducir(Form form, Dictionary<string, string> textos)
        {
            var clavesFaltantes = new List<string>();

            foreach (var control in form.Controls)
            {
                if(control is DataGridView dgv)
                {
                    foreach (DataGridViewColumn column in dgv.Columns)
                    {
                        if (textos.ContainsKey(form.Name + "." + dgv.Name + "." + column.Name))
                        {
                            column.HeaderText = textos[form.Name + "."+ dgv.Name + "." + column.Name];
                        }
                        else
                        {
                            clavesFaltantes.Add(form.Name + "." + dgv.Name + "." + column.Name);
                        }
                    }
                }
                if (control is RadioButton rb)
                {
                    if (textos.ContainsKey(form.Name + "." + rb.Name))
                    {
                        rb.Text = textos[form.Name + "." + rb.Name];
                    }
                    else
                    {
                        clavesFaltantes.Add(form.Name + "." + rb.Name);
                    }
                }
                if (control is Button btn)
                {
                    if (textos.ContainsKey(form.Name + "." + btn.Name))
                    {
                        btn.Text = textos[form.Name + "." + btn.Name];
                    }
                    else
                    {
                        clavesFaltantes.Add(form.Name + "." + btn.Name);
                    }
                }

                if (control is Label lbl)
                {
                    if (textos.ContainsKey(form.Name + "." + lbl.Name))
                    {
                        lbl.Text = textos[form.Name + "." + lbl.Name];
                    }
                    else
                    {
                        clavesFaltantes.Add(form.Name + "." + lbl.Name);
                    }
                }

                if (control is MenuStrip menu)
                {
                    foreach (ToolStripMenuItem item in menu.Items)
                    {
                        if (textos.ContainsKey(form.Name + "." + item.Name))
                        {
                            item.Text = textos[form.Name + "." + item.Name];
                        }
                        else
                        { 
                            clavesFaltantes.Add(form.Name + "." + item.Name);
                        }
                    }

                    foreach (ToolStripMenuItem item in menu.Items)
                    {
                        foreach (ToolStripMenuItem subItem in item.DropDownItems)
                        {
                            if (textos.ContainsKey(form.Name + "." + subItem.Name))
                            {
                                subItem.Text = textos[form.Name + "." + subItem.Name];
                            }
                            else
                            {
                                clavesFaltantes.Add(form.Name + "." + subItem.Name);
                            }
                        }
                    }
                }
            }
            if (clavesFaltantes.Count > 0)
                Idioma.GestorIdioma_64PR.GetInstance.RegistrarClavesFaltantes(clavesFaltantes);
        }

        ///Traduce solo las columnas de UNA grilla puntual. Se usa despues de un rebind (DataSource = ...),
        ///que regenera las columnas autogeneradas y pisa la traduccion previa. A diferencia de Traducir(),
        ///no toca el resto de los controles del formulario (evita pisar labels con contenido dinamico).
        public static void TraducirGrilla(Form form, DataGridView dgv, Dictionary<string, string> textos)
        {
            if (textos == null || textos.Count == 0) return;

            var clavesFaltantes = new List<string>();
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                string clave = form.Name + "." + dgv.Name + "." + column.Name;
                if (textos.ContainsKey(clave))
                    column.HeaderText = textos[clave];
                else
                    clavesFaltantes.Add(clave);
            }

            if (clavesFaltantes.Count > 0)
                Idioma.GestorIdioma_64PR.GetInstance.RegistrarClavesFaltantes(clavesFaltantes);
        }

        ///Traduce un mensaje de error/negocio. Las BLL/Mappers lanzan excepciones cuyo Message es una CLAVE
        ///de idioma (ej. "err_Contrato_NoDisponible"); si el texto no es una clave conocida se muestra tal cual.
        public static string TraducirMensaje(Dictionary<string, string> textos, string claveOMensaje)
        {
            if (textos != null && claveOMensaje != null && textos.ContainsKey(claveOMensaje))
                return textos[claveOMensaje];
            return claveOMensaje;
        }

        ///Texto visible de un valor interno (enum / estado guardado en BD, ej. EN_REPARACION) SIN tocar el valor:
        ///busca la clave de idioma "<prefijo>_<VALOR>" (ej. "estadoReporte_EN_REPARACION"). Si falta la clave,
        ///la registra como faltante y muestra el valor con espacios en vez de "_", nunca el valor crudo.
        public static string TraducirValor(Dictionary<string, string> textos, string prefijo, object valor)
        {
            if (valor == null) return string.Empty;

            string clave = prefijo + "_" + valor;
            if (textos != null && textos.ContainsKey(clave))
                return textos[clave];

            if (textos != null && textos.Count > 0)
                Idioma.GestorIdioma_64PR.GetInstance.RegistrarClavesFaltantes(new List<string> { clave });
            return valor.ToString().Replace('_', ' ');
        }

        ///Carga un ComboBox con los valores de un enum: muestra el texto traducido (TraducirValor) y guarda el enum
        ///como SelectedValue (DisplayMember/ValueMember, mismo patron que FrmRegistrarDevolucionJP86). Conserva la
        ///seleccion al recargar, asi se puede llamar desde ActualizarIdioma (Observer).
        public static void CargarComboValores<TEnum>(ComboBox cmb, Dictionary<string, string> textos, string prefijo) where TEnum : struct
        {
            object seleccionado = cmb.SelectedValue;

            cmb.DisplayMember = "Value";
            cmb.ValueMember = "Key";
            cmb.DataSource = Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Select(valor => new KeyValuePair<TEnum, string>(valor, TraducirValor(textos, prefijo, valor)))
                .ToList();

            if (seleccionado != null)
                cmb.SelectedValue = seleccionado;
        }

        ///Muestra traducida una columna de valores internos de una grilla (solo presentacion, via CellFormatting):
        ///el valor de la celda / DataBoundItem sigue siendo el enum original. obtenerTextos se evalua en cada pintado,
        ///asi toma el idioma vigente; tras un cambio de idioma alcanza con Invalidate() de la grilla.
        public static void TraducirColumnaValores(DataGridView dgv, string nombreColumna, string prefijo, Func<Dictionary<string, string>> obtenerTextos)
        {
            dgv.CellFormatting += (s, e) =>
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.ColumnIndex >= dgv.Columns.Count) return;
                if (dgv.Columns[e.ColumnIndex].Name != nombreColumna) return;
                if (e.Value == null) return;

                e.Value = TraducirValor(obtenerTextos(), prefijo, e.Value);
                e.FormattingApplied = true;
            };
        }

        ///Prefijos de clave para TraducirValor (valores internos de RFN2: BE.EstadoReporteJP86, EstadoVehiculoJP86, ...)
        public const string PrefijoEstadoReporte = "estadoReporte";
        public const string PrefijoEstadoVehiculo = "estadoVehiculo";
        public const string PrefijoFuenteReporte = "fuenteReporte";
        public const string PrefijoCriticidadReporte = "criticidadReporte";
        public const string PrefijoModalidadReparacion = "modalidadReparacion";

        ///Columnas de valores internos de una grilla de BE.ReporteJP86 (CUN-06/07/08): se usa en las 3 grillas de reportes.
        public static void TraducirColumnasReporte(DataGridView dgv, Func<Dictionary<string, string>> obtenerTextos)
        {
            TraducirColumnaValores(dgv, "Estado", PrefijoEstadoReporte, obtenerTextos);
            TraducirColumnaValores(dgv, "Fuente", PrefijoFuenteReporte, obtenerTextos);
            TraducirColumnaValores(dgv, "Criticidad", PrefijoCriticidadReporte, obtenerTextos);
            TraducirColumnaValores(dgv, "ModalidadReparacion", PrefijoModalidadReparacion, obtenerTextos);
        }
    }
}
