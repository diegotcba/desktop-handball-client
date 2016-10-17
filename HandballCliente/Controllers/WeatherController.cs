﻿using HandballCliente.Models;
using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HandballCliente.Controllers
{
    public static class WeatherController
    {
        private static List<Provincia> states = new List<Provincia>();
        private static List<City> cities = new List<City>();
        private static List<PronosticoCiudad> forecasts = new List<PronosticoCiudad>();

        public static List<Provincia> mockProvinciaList()
        {
            return null;
        }

        public static void getForecastStates(String endpoint)
        {
            var client = new RestClient(endpoint);

            var request = new RestRequest("/provincias/", Method.GET);

            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                JsonDeserializer deserial = new JsonDeserializer();

                List<Provincia> prov = deserial.Deserialize<List<Provincia>>(response);

                states.Clear();

                states = prov;
            }
        }

        public static void mockForecastStates()
        {
            states.Clear();

            states.Add(new Provincia(1, "Cordoba"));
            states.Add(new Provincia(2, "Santa Fe"));
            states.Add(new Provincia(3, "La Rioja"));
            states.Add(new Provincia(4, "Tucuman"));
            states.Add(new Provincia(5, "Salta"));
            states.Add(new Provincia(6, "Mendoza"));
        }

        public static void fillForecatStates(ComboBox cmb)
        {
            cmb.Items.Clear();

            cmb.DisplayMember = "nombre";
            cmb.ValueMember = "id";
            cmb.DataSource = states;
        }

        public static void getForecastStateCities(String endpoint, string idState)
        {
            var client = new RestClient(endpoint);

            var request = new RestRequest("/provincias/{id}/ciudades", Method.GET);
            request.AddUrlSegment("id", idState);

            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                JsonDeserializer deserial = new JsonDeserializer();

                List<City> wsCities = deserial.Deserialize<List<City>>(response);

                cities = wsCities;
            }
        }

        public static void mockForecastStateCities()
        {
            cities.Clear();

            cities.Add(new City(1, "Alamafuerte", 1));
            cities.Add(new City(2, "Alta Gracia", 1));
            cities.Add(new City(3, "Bell Ville", 1));
            cities.Add(new City(4, "Cordoba", 1));
            cities.Add(new City(5, "Cordoba Observatorio", 1));
            cities.Add(new City(6, "Coronel Moldes", 1));
            cities.Add(new City(7, "Cosquín", 1));
            cities.Add(new City(8, "Cruz del Eje", 1));
            cities.Add(new City(9, "La Carlota", 1));
            cities.Add(new City(10, "Laboulaye", 1));
            cities.Add(new City(11, "Marcos Juárez", 1));
            cities.Add(new City(12, "Mina Clavero", 1));
            cities.Add(new City(13, "Pilar Obs.", 1));
            cities.Add(new City(14, "Río Cuarto", 1));
            cities.Add(new City(15, "Río Tercero", 1));
            cities.Add(new City(16, "San Francisco", 1));
            cities.Add(new City(17, "Villa Dolores", 1));
            cities.Add(new City(18, "Villa General Belgrano", 1));
            cities.Add(new City(19, "Villa Huidobro", 1));
            cities.Add(new City(20, "Villa María", 1));
            cities.Add(new City(21, "Villa María Del Río Seco", 1));
        }

        public static ListViewItem[] fillSelectedStateCities(long id)
        {
            ListViewItem[] result = new ListViewItem[cities.Count];
            int i = 0;
            string[] arr;
            foreach (City item in cities)
            {
                arr = new string[2];
                arr[0] = item.id.ToString();
                arr[1] = item.nombre;
                result[i] = new ListViewItem(arr);
                i++;
            }

            return result;
        }

        public static void getForecastCityForecast(string endpoint, string cityId)
        {
            var client = new RestClient(endpoint);

            var request = new RestRequest("/pronosticos/", Method.GET);
            //request.AddUrlSegment("id", ((Provincia)cmbWeatherStates.SelectedItem).id.ToString());

            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                JsonDeserializer deserial = new JsonDeserializer();

                List<PronosticoCiudad> forecast = deserial.Deserialize<List<PronosticoCiudad>>(response);

                forecasts = forecast;

                //HandballMatch.getInstance().weatherForecast = forecast;
            }
        }

        public static void mockForecastCityForecast()
        {
            PronosticoCiudad forecastCity = new PronosticoCiudad();

            forecastCity.id = 1;
            forecastCity.ciudad = "Almafuerte";
            forecastCity.fechaHoraPronosticoOficial = "18:00hs";
            List<PronosticoSemana> weeksForecast = new List<PronosticoSemana>();

            PronosticoSemana forecastWeek = new PronosticoSemana();
            forecastWeek.diaSemana = "Hoy Miercoles";
            forecastWeek.diaMes = "23 Sep";
            List<PronosticoDiario> weekForecast = new List<PronosticoDiario>();

            PronosticoDiario forecastDay = new PronosticoDiario();

            forecastDay.parteDia = "Mañana";
            forecastDay.icono = "inestable";
            forecastDay.tipoTemperatura = "Min";
            forecastDay.temperatura = 10;
            forecastDay.simboloTiempo = "°C";
            forecastDay.horaPronostico = "6hs";
            forecastDay.textoPronostico = "Cielo nublado...";

            weekForecast.Add(forecastDay);

            forecastDay = new PronosticoDiario();

            weekForecast.Add(forecastDay);

            forecastWeek.pronostico = weekForecast;

            weeksForecast.Add(forecastWeek);

            forecastCity.pronosticoSemana = weeksForecast;

        }

        public static ListViewItem[] fillForecast(long id)
        {
            ListViewItem[] result = new ListViewItem[cities.Count];
            int i = 0;
            string[] arr;
            foreach (PronosticoCiudad item in forecasts)
            {
                arr = new string[2];
                arr[0] = item.id.ToString();
                arr[1] = item.ciudad;
                result[i] = new ListViewItem(arr);
                i++;
            }

            return result;
        }


    }
}
