using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace Argos_Desktop.Model
{
    class ControleEstado
    {
        private SerialPort _porta;

        public ControleEstado(SerialPort porta)
        {
            _porta = porta;
        }
    }

    //private string LerDadosDaPorta()
    //{
    //    var data = new byte[_porta.BytesToRead];
    //    _porta.Read(data, 0, data.Length);

    //    var resultado = Encoding.UTF8.GetString(data);

    //    return resultado;
    //}
}
