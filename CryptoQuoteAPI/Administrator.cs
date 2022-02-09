using System;
using System.Collections.Generic;
using System.IO;

namespace CryptoQuoteAPI
{
    public class Administrator
    {
        private List<Comission> _comissions;
        private decimal _comissionRate;

        public List<Comission> Comissions
        {
            get
            {
                return _comissions;
            }
            set
            {
                _comissions = value;
            }
        }

        public decimal ComissionRate
        {
            get
            {
                return _comissionRate;
            }
            set
            {
                _comissionRate = value;
            }
        }

        public Administrator()
        {
            _comissions = new List<Comission>();
            _comissionRate = 0.01m;
        }

        public void AddComission(Comission newcomission)
        {
            _comissions.Add(newcomission);
            Save(); 

        }

        public void Save()
        {
            var fileName = "Comissions.json";
            var directory = @"C:\Users\Utilizador\Desktop\Restart2\C#_repositorio\TugaExchange";
            string filePath = Path.Combine(directory, fileName);

            //Criação json a partir da Lista de Coins
            string json = System.Text.Json.JsonSerializer.Serialize(_comissions);
            File.WriteAllText(filePath, json);
        }

        public void Read()
        {
            var fileName = "Comissions.json";
            var directory = @"C:\Users\Utilizador\Desktop\Restart2\C#_repositorio\TugaExchange";
            string filePath = Path.Combine(directory, fileName);

            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Exists)
            {
                var jason = File.ReadAllText(filePath);
                _comissions = System.Text.Json.JsonSerializer.Deserialize<List<Comission>>(jason);
            }

        }
    }
}
