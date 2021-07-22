using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherForecastApp.Models
{
    public class WeatherStackResult 
    {
        private WeatherStackResponse _response;

        public WeatherStackResult(WeatherStackResponse response)
        {
            _response = response;
        }

        public WeatherStackResult(bool result, string errorMessage)
        {
            _isValid = result;
            _errorMessage = errorMessage;
        }


        public WeatherStackResponse Response 
        {

            get { return _response; }
        }

        private bool _isValid;
        public bool IsValid 
        {
            get
            {
                if (_response == null)
                    return _isValid;

                if (_response.ErrorInfo == null)
                    _isValid = true;

                return _isValid;
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get 
            {
                if (_response == null)
                    return _errorMessage;

                if (_response.ErrorInfo != null)
                    _errorMessage = _response.ErrorInfo.Info;

                return _errorMessage;
            }
        }
    }
}
