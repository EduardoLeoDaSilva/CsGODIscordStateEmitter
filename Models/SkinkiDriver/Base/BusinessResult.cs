using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsGOStateEmitter.Models.SkinkiDriver
{
    /// <summary>
    /// Classe usada para transitar entre serviços e controller
    /// </summary>    
    public class BusinessResult<T>
    {
        #region Constructor

        public BusinessResult()
        {
            Messages = new List<MessageResult>();
        }

        public BusinessResult(T data)
        {
            Data = data;
            Messages = new List<MessageResult>();
        }

        #endregion

        public T Data { get; set; }
        public List<CsGOStateEmitter.Models.SkinkiDriver.MessageResult> Messages { get; set; }
        public string Token { get; set; }

        public bool IsValid { get; set; }

    }
}
