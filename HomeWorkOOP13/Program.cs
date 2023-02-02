using System;
using System.Linq;

namespace HomeWorkOOP13
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CarService carService = new("Автоцентр-Владимир на Рокадной", "г.Владимир, ул.Электрозаводская, д.2А");
            Console.WriteLine($"Приветствуем Вас в нашем салоне {carService.Name}");
            carService.StartDay();
        }
    }

    class CarService
    {
        private int _money = 0;
        private Queue<Car> _cars = new();
        private List<Storage> _details = new();
        private CarBuilder _carBuilder = new();
        private Price _price = new();
        private static Random _random = new();

        public CarService(string name, string adress)
        {
            Name = name;
            Adress = adress;

            _details = new List<Storage>()
            {
                new Storage(new Detail("Двигатель"), 3),
                new Storage(new Detail("Цепь ГРМ"), 3),
                new Storage(new Detail("Сцепление"), 2),
            };

            _cars = _carBuilder.Build(10);
        } 

        public string Name { get; private set; }
        public string Adress { get; private set; }

        public void StartDay()
        {
            const string CommandServeClient = "1";
            const string CommandExit = "2";

            bool isProgramOn = true;

            while (_cars.Count > 0 && isProgramOn)
            {
                Console.Clear();
                Console.WriteLine($"Добрый день.\nВ салоне очередь из {_cars.Count} машин");
                Console.WriteLine($"{CommandServeClient}-Обслужить клиента");
                Console.WriteLine($"{CommandExit}-Выйти");

                string? userInput = Console.ReadLine();

                switch (userInput)
                {
                    case CommandServeClient:
                        ServeClient();
                        break;

                    case CommandExit:
                        isProgramOn = false;
                        break;

                    default:
                        Console.WriteLine("Введите цифру меню");
                        break;
                }
            }

            if(_cars.Count == 0)
            {
                Console.WriteLine("Очередь закончилась");
                Console.WriteLine($"За день Вы заработали: {_money}");
                Console.ReadKey();
            }
        }

        private void ServeClient()
        {
            var client = _cars.Peek();
            bool isClientOn = true;

            while (isClientOn && _cars.Count > 0)
            {
                const string CommandFixCar = "1";
                const string CommandShowStorage = "2";
                const string CommandEnd = "3";

                Console.Clear();
                Console.WriteLine($"Проблема клиента: {client.BrokenDetail.Title}");
                Console.WriteLine($"Цена починки: {CalculateCost()}");
                Console.WriteLine($"Касса: {_money}");
                Console.WriteLine($"Выберите команду:\n{CommandFixCar}-Починить машину\n{CommandShowStorage}-Посмотреть склад\n{CommandEnd}-Выйти");

                string userInput = Console.ReadLine()!;

                switch (userInput)
                {
                    case CommandFixCar:
                        FixCar();
                        isClientOn = false;
                        break;

                    case CommandShowStorage:
                        ShowStorage();
                        break;

                    case CommandEnd:
                        isClientOn = false;
                        break;
                }
            }
        }

        private void FixCar()
        {
            var client = _cars.Peek();

            foreach (Storage detail in _details)
            {
                if(detail.Detail.Title == client.BrokenDetail.Title)
                {
                    if(detail.Quantity > 0)
                    {
                        detail.RemoveQuantity();

                        if (DoMistake())
                        {
                            Console.WriteLine("Слесарь по ошибке заменил не ту деталь, нужно заплатить штраф");
                            Console.ReadKey();
                            _cars.Dequeue();
                        }
                        else
                        {
                            Console.WriteLine("Успешная починка");
                            _money += CalculateCost();
                            _cars.Dequeue();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Нет такой детали, нужно заплатить штраф");
                        Penalty();
                        _cars.Dequeue();
                    }
                }
            }
        }

        private void ShowStorage()
        {
            int index = 1;

            Console.Clear();

            foreach (Storage storage in _details)
            {
                Console.WriteLine($"{index++}.{storage.Detail.Title}, количество: {storage.Quantity}");
            }

            Console.ReadKey();
        }

        private int CalculateCost()
        {
            if(_cars.Count > 0)
            {
                var client = _cars.Peek();
                return _price.ReturnCost(client.BrokenDetail);
            }
            else
            {
                Console.WriteLine("Клиентов больше нет");
                return 0;
            }
        }

        private void Penalty()
        {
            _money -= CalculateCost();
            Console.ReadKey();
        }

        private bool DoMistake()
        {
            if(Chance())
            {
                Penalty();
                Console.WriteLine($"Штраф: {CalculateCost()}");
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool Chance()
        {
            int minNumber = 1;
            int maxNumber = 100;
            int chance = 10;
            int randomNumber = _random.Next(minNumber, maxNumber);

            return randomNumber < chance;
        }
    }

    class Storage
    {
        public Storage(Detail detail, int quantity)
        {
            Detail = detail;
            Quantity = quantity;
        }

        public Detail Detail { get; }
        public int Quantity { get; private set; }

        public void RemoveQuantity()
        {
            Quantity--;
        }
    }

    class Car
    {
        public Car(Detail detail)
        {
            BrokenDetail = detail;
        }

        public Detail BrokenDetail { get; }
    }

    class CarBuilder
    {
        private static Random _random = new();
        private List<Detail> _brokenDetails;

        public CarBuilder()
        {
            _brokenDetails = new List<Detail>()
            {
                new Detail("Двигатель"),
                new Detail("Цепь ГРМ"),
                new Detail("Сцепление"),
            };
        }

        public Queue<Car> Build(int carCount)
        {
            Queue<Car> cars = new();

            for (int i = 0; i < carCount; i++)
            {
                cars.Enqueue(new Car(CreateRandomBrokenDetail()));
            }

            return cars;
        }

        public Detail CreateRandomBrokenDetail()
        {
            int randomIndex = _random.Next(_brokenDetails.Count);
            Detail detail = _brokenDetails[randomIndex];
            return new Detail(detail.Title);
        }
    }

    class Detail
    {
        public Detail(string title)
        {
            Title = title;
        }

        public string Title { get; private set; }
    }

    class Price
    {
        Dictionary<string, int> _price = new();

        public Price()
        {
            _price.Add("Двигатель", 10000);
            _price.Add("Сцепление", 8000);
            _price.Add("Цепь ГРМ", 4000);
        }

        public int ReturnCost(Detail detail)
        {
            if (_price.TryGetValue(detail.Title, out int price))
            {
                return price;
            }

            return 0;
        }
    }
}