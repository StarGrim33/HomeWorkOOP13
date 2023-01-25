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

        public CarService(string name, string adress)
        {
            Name = name;
            Adress = adress;

            _details = new List<Storage>()
            {
                new Storage(new Detail("Двигатель", 100000), 5),
                new Storage(new Detail("Ремень ГРМ", 3000), 10),
                new Storage(new Detail("Колодки", 2000), 50),
                new Storage(new Detail("Рулевая рейка", 4000), 5),
                new Storage(new Detail("Сцепление", 8000), 7),
                new Storage(new Detail("Тормозной шланг", 500), 100),
                new Storage(new Detail("Масло", 800), 100),
                new Storage(new Detail("Глушитель", 1000), 10),
                new Storage(new Detail("Жидкость гур", 1200), 15), 
                new Storage(new Detail("Дверь", 15000), 5),
                new Storage(new Detail("Крыло", 6000), 5),
                new Storage(new Detail("Лобовое стекло", 12000), 3)
            };

            _cars.Enqueue(new Car("Иван", "Порвался ремень ГРМ"));
            _cars.Enqueue(new Car("Александр", "Сгорел двигатель"));
            _cars.Enqueue(new Car("Влад", "Сгорело сцепление"));
            _cars.Enqueue(new Car("Михаил", "Поменять моторное масло"));
            _cars.Enqueue(new Car("Анатолий", "Заменить дверь после аварии"));
            _cars.Enqueue(new Car("Дмитрий", "Разбито лобовое стекло"));
            _cars.Enqueue(new Car("Олег", "Заменить жидкость ГУР"));
            _cars.Enqueue(new Car("Роман", "Заменить рулевую рейку"));
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
            var client = _cars.Peek();
            bool isClientOn = true;

            while(isClientOn)
            {
                const string CommandFixCar = "1";
                const string CommandShowStorage = "2";
                const string CommandEnd = "3";

                Console.Clear();
                Console.WriteLine($"Клиент: {client.Name}, проблема: {client.Problem}");

                Console.WriteLine($"Выберите команду:\n{CommandFixCar}-Починить машину\n{CommandShowStorage}-Посмотреть склад\n{CommandEnd}-Закончить");

                string userInput = Console.ReadLine()! /*?? String.Empty*/;

                switch (userInput)
                {
                    case CommandFixCar:
                        FixCar();
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

        }

        private void ShowStorage()
        {
            int index = 1;

            Console.Clear();

            foreach(Storage storage in _details)
            {
                Console.WriteLine($"{index++}.{storage.Detail.Name}, количество: {storage.Quantity}");
            }

            Console.ReadKey();
        }

        private void CalculateCost()
        {
            
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
    }

    class Car
    {
        public Car(string name, string problem)
        {
            Name = name;
            Problem = problem;
        }

        public string Name { get; private set; }
        public string Problem { get; private set; }
    }

    class Detail
    {
        public Detail(string name, int cost)
        {
            Name = name;
            Cost = cost;
        }

        public string Name { get; private set; }
        public int Cost { get; private set; }
    }
}