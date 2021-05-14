using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Comun.Modelos
{
    public class Resultado<T>
    {
        public Resultado() { }

        public bool Correcto { get; set; }
        public string[] Errores { get; set; }        
        public T Datos { get; set; }

        internal Resultado(bool correcto, IEnumerable<string> errores,T datos)
        {
            Correcto = correcto;
            Errores = errores.ToArray();
            Datos = datos;
        }

        public static Resultado<T> Success(T datos)
        {
            return new Resultado<T>(true, new string[] { },datos);
        }

        public static Resultado<T> Failure(IEnumerable<string> errors)
        {
            return new Resultado<T>(false, errors, default(T));
        }

        public static Resultado<T> Failure(string error)
        {
            return new Resultado<T>(false, new List<string> { error }, default(T));
        }

    }
}
