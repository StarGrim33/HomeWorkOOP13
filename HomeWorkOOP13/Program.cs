namespace HomeWorkOOP13
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CarService carService = new("Автоцентр-Владимир на Рокадной", "г.Владимир, ул.Электрозаводская, д.2А");
            Console.WriteLine($"Приветствуем Вас в нашем салоне {carService.Name}");
            carService.Menu();
        }
    }

    class CarService
    {
        private static Random _random = new();
        private int _money = 0;
        private Queue<Car> _cars = new();
        private List<Storage> _details = new();

        public CarService(string name, string adress)
        {
            CarBuilder _carBuilder = new();
            Name = name;
            Adress = adress;

            _details = new List<Storage>()
            {
                new Storage(new Detail("Двигатель", 10000), 3),
                new Storage(new Detail("Цепь ГРМ", 8000), 3),
                new Storage(new Detail("Сцепление", 4000), 2),
            };

            _cars = _carBuilder.Build(10);
        }

        public string Name { get; private set; }
        public string Adress { get; private set; }

        public void Menu()
        {
            const string CommandServeClient = "1";
            const string CommandExit = "2";

            bool isProgramOn = true;

            while (isProgramOn && _cars.Count > 0)
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
        }

        private void ServeClient()
        {
            bool isClientOn = true;

            if (_cars.Count >= 0)
            {
                var car = _cars.Dequeue();

                while (isClientOn)
                {
                    const string CommandFixCar = "1";
                    const string CommandShowStorage = "2";
                    const string CommandEnd = "3";

                    Console.Clear();
                    Console.WriteLine($"Проблема клиента: {car.BrokenDetail.Title}");
                    Console.WriteLine($"Цена починки: {CalculateCost(car)}");
                    Console.WriteLine($"Касса: {_money}");
                    Console.WriteLine($"Выберите команду:\n{CommandFixCar}-Починить машину\n{CommandShowStorage}-Посмотреть склад\n{CommandEnd}-Выйти");

                    string userInput = Console.ReadLine()!;

                    switch (userInput)
                    {
                        case CommandFixCar:
                            FixCar(car);
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

            if (_cars.Count == 0)
            {
                Console.WriteLine("Очередь закончилась");
                Console.WriteLine($"За день Вы заработали: {_money}");
                Console.ReadKey();
            }
        }

        private void FixCar(Car car)
        {
            foreach (Storage detail in _details)
            {
                if (detail.Detail.Title == car.BrokenDetail.Title)
                {
                    if (detail.Quantity > 0)
                    {
                        detail.RemoveQuantity();

                        if (IsWorkerDidMistake(car))
                        {
                            Console.WriteLine("Слесарь по ошибке заменил не ту деталь, нужно заплатить штраф");
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.WriteLine("Успешная починка");
                            _money += CalculateCost(car);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Нет такой детали, нужно заплатить штраф");
                        Penalise(car);
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

        private int CalculateCost(Car car)
        {
            if (_cars.Count >= 0)
            {
                return car.BrokenDetail.Cost;
            }
            else
            {
                Console.WriteLine("Клиентов больше нет");
                return 0;
            }
        }

        private void Penalise(Car car)
        {
            _money -= CalculateCost(car);
            Console.ReadKey();
        }

        private bool IsWorkerDidMistake(Car car)
        {
            if (IsChance())
            {
                Penalise(car);
                Console.WriteLine($"Штраф: {CalculateCost(car)}");
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsChance()
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
                new Detail("Двигатель", 10000),
                new Detail("Цепь ГРМ", 8000),
                new Detail("Сцепление", 4000),
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
            return new Detail(detail.Title, detail.Cost);
        }
    }

    class Detail
    {
        public Detail(string title, int cost)
        {
            Title = title;
            Cost = cost;
        }

        public string Title { get; private set; }
        public int Cost { get; private set; }
    }
}