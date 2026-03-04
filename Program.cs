using System;
using System.Collections.Generic;
using System.Linq;

namespace MagicalDungeon
{
    enum School { Fire, Water, Air, Light, Divine }

    class Enemy
    {
        public string Name { get; }
        public int HP { get; set; }
        public int MaxHP { get; }
        public Dictionary<School, double> Resistances { get; }
        public int MinDamage { get; }
        public int MaxDamage { get; }
        public bool IsBoss { get; }

        public Enemy(string name, int hp, int minDmg, int maxDmg,
                     Dictionary<School, double> resist, bool isBoss = false)
        {
            Name = name;
            HP = hp;
            MaxHP = hp;
            MinDamage = minDmg;
            MaxDamage = maxDmg;
            Resistances = resist ?? new Dictionary<School, double>();
            IsBoss = isBoss;
        }

        public int Attack(Random rnd) => rnd.Next(MinDamage, MaxDamage + 1);
        public double GetResistMod(School s) => Resistances.ContainsKey(s) ? Resistances[s] : 1.0;
    }

    class Player
    {
        public int HP { get; set; }
        public int MaxHP { get; }
        public int Mana { get; set; }
        public int MaxMana { get; }
        public Dictionary<School, int> Experience { get; }
        public bool BarrierActive { get; set; }

        public int HealPotions { get; set; }
        public int ManaPotions { get; set; }
        public int ArmorPotions { get; set; }
        public int StrangePotions { get; set; }
        public int ArmorCharges { get; set; }

        public Player(int hp, int mana)
        {
            MaxHP = hp;
            HP = hp;
            MaxMana = mana;
            Mana = mana;
            Experience = new Dictionary<School, int>();
            foreach (School s in Enum.GetValues(typeof(School)))
                Experience[s] = 0;
            BarrierActive = false;
            HealPotions = ManaPotions = ArmorPotions = StrangePotions = 0;
            ArmorCharges = 0;
        }

        public void RegenerateMana(int amount) =>
            Mana = Math.Min(Mana + amount, MaxMana);

        public void ResetBarrier() => BarrierActive = false;
    }

    class Program
    {
        static Random rnd = new Random();

        static ConsoleColor GetSchoolColor(School s)
        {
            return s switch
            {
                School.Fire => ConsoleColor.Red,
                School.Water => ConsoleColor.DarkBlue,
                School.Air => ConsoleColor.Cyan,
                School.Light => ConsoleColor.Yellow,
                School.Divine => ConsoleColor.Magenta,
                _ => ConsoleColor.Gray
            };
        }

        static void PressAnyKey(string msg = "Нажми Enter, чтобы продолжить...")
        {
            Console.WriteLine(msg);
            Console.ReadLine();
        }

        static void Main(string[] args)
        {
            bool restart;
            do
            {
                restart = false;
                PlayGame();
                Console.WriteLine("\n1 - завершить, 2 - перезапустить");
                if (Console.ReadLine() == "2") restart = true;
            } while (restart);
            Console.WriteLine("Спасибо за игру!");
        }

        static void PlayGame()
        {
            Player player = new Player(100, 100);

            // --- Вступление ---
            Console.Clear();
            Console.WriteLine("Добро пожаловать в подземелье, искатель приключений!");
            Console.WriteLine("Ты - молодой волшебник, который по заданию коллегии магов должен сразить бывшего послушника коллегии.");
            Console.WriteLine("Он изменник, который познал магию и не смог устоять перед соблазном захватить библиотеку подземелья, впитывая знания ради своей выгоды!");
            PressAnyKey();

            Console.Clear();
            Console.WriteLine("В твоём распоряжении есть 4 школы заклинаний! Каждая школа по-разному влияет на приспешников злобного колдуна - его големов!");
            Console.WriteLine("Коллегия не смогла тебе подробно рассказать о свойствах големов, поэтому тебе придётся узнавать всё на ходу! Успехов!");
            PressAnyKey();

            // --- Битва с тремя големами ---
            Console.Clear();
            Console.WriteLine("Пробираясь через катакомбы, ты наконец добрался до двери, которая слабо сияет светом.");
            Console.WriteLine("Твоё магическое чутьё подсказывает, что за дверью находятся противники! Приготовься к бою!");
            PressAnyKey("Нажми Enter, чтобы толкнуть дверь заклинанием воздуха...");

            var stoneGolem = new Enemy("Каменный голем", 70, 5, 15, new Dictionary<School, double>
            {
                [School.Fire] = 0.7,
                [School.Water] = 1.0,
                [School.Air] = 1.1,
                [School.Light] = 1.0,
                [School.Divine] = 1.0
            });
            var ironGolem = new Enemy("Железный голем", 80, 5, 15, new Dictionary<School, double>
            {
                [School.Fire] = 0.85,
                [School.Water] = 1.15,
                [School.Air] = 1.05,
                [School.Light] = 1.0,
                [School.Divine] = 1.0
            });
            var darkGolem = new Enemy("Тёмный голем", 60, 5, 15, new Dictionary<School, double>
            {
                [School.Fire] = 1.1,
                [School.Water] = 0.8,
                [School.Air] = 0.8,
                [School.Light] = 1.3,
                [School.Divine] = 1.0
            });

            List<Enemy> golems = new List<Enemy> { stoneGolem, ironGolem, darkGolem };

            bool victory = FightWithGolems(player, golems);
            if (!victory)
            {
                Console.WriteLine("Ты погиб... Всё начинается заново.");
                PressAnyKey();
                return;
            }

            // --- После големов ---
            Console.Clear();
            Console.WriteLine("Битва тебя изрядно измотала, искатель приключений! Но в этой битве ты обрёл ценные знания!");
            Console.WriteLine("Вот список магических эссенций, которые ты смог обрести в битве!");

            foreach (School s in new[] { School.Fire, School.Water, School.Air, School.Light, School.Divine })
            {
                Console.ForegroundColor = GetSchoolColor(s);
                Console.WriteLine($"{s}: {player.Experience[s]} очков");
            }
            Console.ResetColor();

            Console.WriteLine("\nТы чувствуешь, как приближается он... Отступник... Предатель коллегии магов!");

            School bossEyeColor = (School)rnd.Next(0, 4);
            Console.Write("Его глаза светятся ");
            Console.ForegroundColor = GetSchoolColor(bossEyeColor);
            switch (bossEyeColor)
            {
                case School.Fire: Console.Write("красным"); break;
                case School.Water: Console.Write("тёмно-синим"); break;
                case School.Air: Console.Write("голубым"); break;
                case School.Light: Console.Write("жёлтым"); break;
            }
            Console.ResetColor();
            Console.WriteLine(" цветом!");

            var maxExpSchool = player.Experience.OrderByDescending(kv => kv.Value).First().Key;

            if (maxExpSchool == School.Divine)
            {
                Console.WriteLine("\nВы понимаете, что высшая школа магии позволяет вам скрывать ваши способности.");
                Console.WriteLine("Хотите ли вы утихомирить вашего оппонента или сразиться с ним в честном бою?");
                Console.WriteLine("1 - Начать бой с жалким трусом!!");
                Console.WriteLine("2 - Призвать святого голема!!");
                string choice = Console.ReadLine();
                if (choice == "2")
                {
                    Console.WriteLine("\nСвятой голем наносит удар за ударом по отступнику и побеждает его!");
                    Console.WriteLine("Вы справились и понимаете, что всё было не зря!");
                    PressAnyKey();
                    return;
                }
            }
            else if (maxExpSchool == bossEyeColor)
            {
                Console.WriteLine("\nЯ чувствую, что ты достиг определённых успехов, жалкий червь! Так уж и быть, я выслушаю тебя! Говори, что тебе надо?!");
                Console.WriteLine("1 - Коллегия не прощает предателей! Сейчас ты поплатишься за своё отступничество! (бой)");
                Console.WriteLine("2 - Послушай меня! Я понимаю соблазн приобретения силы, я прошёл тот же путь, что и ты! Вернись в коллегию, остуди свою ярость и пыл! (мир)");
                string choice = Console.ReadLine();
                if (choice == "2")
                {
                    Console.WriteLine("\nДа... Я чувствую, что мы с тобой адепты одной школы... Что ж... Может и стоит к тебе прислушаться!");
                    Console.WriteLine("Так и быть! Мы вернёмся в коллегию!");
                    Console.WriteLine("Вам невероятно повезло! Вы смогли уговорить отступника! Наконец-то баланс восстановлен и ценные знания подземной библиотеки доступны коллегии!");
                    PressAnyKey();
                    return;
                }
            }
            else
            {
                Console.WriteLine("\nЖалкий червяк, что приполз ко мне на коленях! Я ни грамма не чувствую в тебе магических сил! Ха-ха! Приготовься умереть!");
            }

            var boss = new Enemy("Злой колдун", 150, 10, 20, new Dictionary<School, double>
            {
                [School.Fire] = 0.7,
                [School.Water] = 0.7,
                [School.Air] = 0.7,
                [School.Light] = 0.7,
                [School.Divine] = 0.7
            }, isBoss: true);

            bool bossVictory = FightWithBoss(player, boss);
            if (bossVictory)
                Console.WriteLine("Ты победил злого колдуна! Подземная библиотека спасена, и коллегия магов торжествует!");
            else
                Console.WriteLine("Ты погиб... Всё начинается заново.");
            PressAnyKey();
        }

        static bool FightWithGolems(Player player, List<Enemy> golems)
        {
            while (golems.Any(g => g.HP > 0) && player.HP > 0)
            {
                Console.Clear();
                Console.WriteLine("=== Битва с големами ===");
                Console.WriteLine($"Игрок: HP {player.HP}/{player.MaxHP}  Мана: {player.Mana}/{player.MaxMana}");
                if (player.ArmorCharges > 0)
                    Console.WriteLine($"Заряды стойкости: {player.ArmorCharges}");
                Console.WriteLine();

                for (int i = 0; i < golems.Count; i++)
                {
                    var g = golems[i];
                    Console.WriteLine($"{i + 1}. {g.Name}: HP {g.HP}/{g.MaxHP} {(g.HP <= 0 ? "(ПОВЕРЖЕН)" : "")}");
                }

                bool turnUsed = false;
                while (!turnUsed)
                {
                    Console.WriteLine("\nВыбери действие:");
                    Console.WriteLine("1 - Огненный шар (огонь, 20 маны)");
                    Console.WriteLine("2 - Ледяной шип (вода, 20 маны)");
                    Console.WriteLine("3 - Воздушный удар (воздух, 20 маны)");
                    Console.WriteLine("4 - Святой луч (свет, 20 маны)");
                    Console.WriteLine("5 - Защитный барьер (высшая, 10 маны)");
                    Console.WriteLine("6 - Святое лечение (высшая, 10 маны)");
                    Console.WriteLine($"7 - Зелье лечения (+30 HP) [{player.HealPotions}]");
                    Console.WriteLine($"8 - Зелье маны (+30 MP) [{player.ManaPotions}]");
                    Console.WriteLine($"9 - Зелье стойкости (3 заряда) [{player.ArmorPotions}]");
                    Console.WriteLine($"0 - Странное зелье (??? ) [{player.StrangePotions}]");
                    Console.WriteLine("10 - Магическая медитация (пропустить ход)");
                    Console.Write("Твой выбор: ");

                    string input = Console.ReadLine();
                    if (!int.TryParse(input, out int choice))
                    {
                        Console.WriteLine("Неверный ввод. Попробуй ещё.");
                        continue;
                    }

                    if (choice >= 1 && choice <= 6) // заклинания
                    {
                        if (choice == 5 && player.ArmorCharges > 0)
                        {
                            Console.WriteLine("Простофиля! Береги ману! Ты и так выпил зелье стойкости!");
                            turnUsed = true;
                            break;
                        }

                        School school;
                        int manaCost;
                        bool isAttack = choice <= 4;
                        if (choice == 1) { school = School.Fire; manaCost = 20; }
                        else if (choice == 2) { school = School.Water; manaCost = 20; }
                        else if (choice == 3) { school = School.Air; manaCost = 20; }
                        else if (choice == 4) { school = School.Light; manaCost = 20; }
                        else if (choice == 5) { school = School.Divine; manaCost = 10; }
                        else { school = School.Divine; manaCost = 10; }

                        if (player.Mana < manaCost)
                        {
                            Console.WriteLine("Недостаточно маны!");
                            continue;
                        }

                        Enemy target = null;
                        if (isAttack)
                        {
                            Console.Write("Выбери цель (номер голема): ");
                            if (!int.TryParse(Console.ReadLine(), out int targetIndex) ||
                                targetIndex < 1 || targetIndex > golems.Count ||
                                golems[targetIndex - 1].HP <= 0)
                            {
                                Console.WriteLine("Неверная цель.");
                                continue;
                            }
                            target = golems[targetIndex - 1];
                        }

                        player.ResetBarrier();
                        player.Mana -= manaCost;
                        player.Experience[school]++;

                        if (isAttack)
                        {
                            int baseDamage = 20;
                            double mod = target.GetResistMod(school);
                            int damage = (int)Math.Round(baseDamage * mod);
                            target.HP -= damage;
                            Console.WriteLine($"Ты используешь {school} и наносишь {damage} урона {target.Name}!");

                            if (target.HP <= 0 && !target.IsBoss)
                                DropPotion(player);
                        }
                        else if (choice == 5)
                        {
                            player.BarrierActive = true;
                            Console.WriteLine("Ты создаёшь защитный барьер!");
                        }
                        else
                        {
                            player.HP = Math.Min(player.HP + 20, player.MaxHP); // УВЕЛИЧЕНО до 20
                            Console.WriteLine($"Ты лечишься на 20 HP. Теперь у тебя {player.HP} HP.");
                        }

                        turnUsed = true;
                    }
                    else if (choice == 7 || choice == 8 || choice == 9 || choice == 0) // зелья
                    {
                        bool used = false;
                        if (choice == 7 && player.HealPotions > 0)
                        {
                            player.HP = Math.Min(player.HP + 30, player.MaxHP);
                            player.HealPotions--;
                            Console.WriteLine($"Ты выпил зелье лечения! HP теперь {player.HP}.");
                            used = true;
                        }
                        else if (choice == 8 && player.ManaPotions > 0)
                        {
                            player.Mana = Math.Min(player.Mana + 30, player.MaxMana);
                            player.ManaPotions--;
                            Console.WriteLine($"Ты выпил зелье маны! Мана теперь {player.Mana}.");
                            used = true;
                        }
                        else if (choice == 9 && player.ArmorPotions > 0)
                        {
                            player.ArmorCharges = 3;
                            player.ArmorPotions--;
                            Console.WriteLine("Ты выпил зелье стойкости! Теперь ты неуязвим на 3 удара (барьер недоступен).");
                            used = true;
                        }
                        else if (choice == 0 && player.StrangePotions > 0)
                        {
                            int effect = rnd.Next(4);
                            player.StrangePotions--;
                            switch (effect)
                            {
                                case 0:
                                    player.HP = Math.Min(player.HP + 30, player.MaxHP);
                                    Console.WriteLine($"Ты выпил странное зелье и оно оказалось лечащим! HP теперь {player.HP}.");
                                    break;
                                case 1:
                                    player.Mana = Math.Min(player.Mana + 30, player.MaxMana);
                                    Console.WriteLine($"Ты выпил странное зелье и оно оказалось магическим! Мана теперь {player.Mana}.");
                                    break;
                                case 2:
                                    player.ArmorCharges = 3;
                                    Console.WriteLine("Ты выпил странное зелье и оно оказалось зельем стойкости! Теперь 3 заряда.");
                                    break;
                                case 3:
                                    player.HP -= 20;
                                    Console.WriteLine("О нет! Это было не зелье, а ядовитая микстура!! Надо было чаще посещать уроки алхимии в коллегии! Вам нанесено 20 урона.");
                                    if (player.HP <= 0)
                                        Console.WriteLine("Ты погиб от отравления...");
                                    break;
                            }
                            used = true;
                        }
                        else
                        {
                            Console.WriteLine("У тебя нет такого зелья!");
                            continue;
                        }

                        if (used)
                        {
                            turnUsed = true;
                            player.ResetBarrier();
                        }
                    }
                    // добавление обработки медитации
                    else if (choice == 10)
                    {
                        Console.WriteLine("Ты погружаешься в медитацию, восстанавливая внутренний баланс...");
                        turnUsed = true;
                        player.ResetBarrier();
                    }
                    else
                    {
                        Console.WriteLine("Неверный ввод. Попробуй ещё.");
                    }
                }

                if (golems.All(g => g.HP <= 0))
                {
                    Console.WriteLine("\nВсе големы повержены! Ты пробился к боссу!");
                    return true;
                }

                var alive = golems.Where(g => g.HP > 0).ToList();
                var attacker = alive[rnd.Next(alive.Count)];
                int dmg = attacker.Attack(rnd);

                if (player.ArmorCharges > 0)
                {
                    player.ArmorCharges--;
                    Console.WriteLine($"Зелье стойкости поглощает урон! Осталось зарядов: {player.ArmorCharges}.");
                }
                else if (player.BarrierActive)
                {
                    Console.WriteLine($"{attacker.Name} атакует, но барьер поглощает урон!");
                }
                else
                {
                    player.HP -= dmg;
                    Console.WriteLine($"{attacker.Name} атакует и наносит {dmg} урона!");
                }

                if (player.HP <= 0) return false;

                player.RegenerateMana(10); // ИЗМЕНЕНО с 20 на 10
                Console.WriteLine("Мана восстановлена на 10.");
                PressAnyKey();
            }
            return player.HP > 0;
        }

        static bool FightWithBoss(Player player, Enemy boss)
        {
            while (boss.HP > 0 && player.HP > 0)
            {
                Console.Clear();
                Console.WriteLine("=== Битва с Злым Колдуном ===");
                Console.WriteLine($"Игрок: HP {player.HP}/{player.MaxHP}  Мана: {player.Mana}/{player.MaxMana}");
                if (player.ArmorCharges > 0)
                    Console.WriteLine($"Заряды стойкости: {player.ArmorCharges}");
                Console.WriteLine($"Босс: HP {boss.HP}/{boss.MaxHP}\n");

                bool turnUsed = false;
                while (!turnUsed)
                {
                    Console.WriteLine("Выбери действие:");
                    Console.WriteLine("1 - Огненный шар (20 маны)");
                    Console.WriteLine("2 - Ледяной шип (20 маны)");
                    Console.WriteLine("3 - Воздушный удар (20 маны)");
                    Console.WriteLine("4 - Святой луч (20 маны)");
                    Console.WriteLine("5 - Защитный барьер (10 маны)");
                    Console.WriteLine("6 - Святое лечение (10 маны)");
                    Console.WriteLine($"7 - Зелье лечения (+30 HP) [{player.HealPotions}]");
                    Console.WriteLine($"8 - Зелье маны (+30 MP) [{player.ManaPotions}]");
                    Console.WriteLine($"9 - Зелье стойкости (3 заряда) [{player.ArmorPotions}]");
                    Console.WriteLine($"0 - Странное зелье (??? ) [{player.StrangePotions}]");
                    Console.WriteLine("10 - Магическая медитация (пропустить ход)");
                    Console.Write("Твой выбор: ");

                    string input = Console.ReadLine();
                    if (!int.TryParse(input, out int choice))
                    {
                        Console.WriteLine("Неверный ввод.");
                        continue;
                    }

                    if (choice >= 1 && choice <= 6)
                    {
                        if (choice == 5 && player.ArmorCharges > 0)
                        {
                            Console.WriteLine("Простофиля! Береги ману! Ты и так выпил зелье стойкости!");
                            turnUsed = true;
                            break;
                        }

                        School school;
                        int manaCost;
                        bool isAttack = choice <= 4;
                        if (choice == 1) { school = School.Fire; manaCost = 20; }
                        else if (choice == 2) { school = School.Water; manaCost = 20; }
                        else if (choice == 3) { school = School.Air; manaCost = 20; }
                        else if (choice == 4) { school = School.Light; manaCost = 20; }
                        else if (choice == 5) { school = School.Divine; manaCost = 10; }
                        else { school = School.Divine; manaCost = 10; }

                        if (player.Mana < manaCost)
                        {
                            Console.WriteLine("Недостаточно маны!");
                            continue;
                        }

                        player.ResetBarrier();
                        player.Mana -= manaCost;
                        player.Experience[school]++;

                        if (isAttack)
                        {
                            int baseDamage = 20;
                            double mod = boss.GetResistMod(school);
                            int damage = (int)Math.Round(baseDamage * mod);
                            boss.HP -= damage;
                            Console.WriteLine($"Ты используешь {school} и наносишь {damage} урона боссу!");
                        }
                        else if (choice == 5)
                        {
                            player.BarrierActive = true;
                            Console.WriteLine("Ты создаёшь защитный барьер!");
                        }
                        else
                        {
                            player.HP = Math.Min(player.HP + 20, player.MaxHP); // УВЕЛИЧЕНО до 20
                            Console.WriteLine($"Ты лечишься на 20 HP. Теперь у тебя {player.HP} HP.");
                        }

                        turnUsed = true;
                    }
                    else if (choice == 7 || choice == 8 || choice == 9 || choice == 0)
                    {
                        bool used = false;
                        if (choice == 7 && player.HealPotions > 0)
                        {
                            player.HP = Math.Min(player.HP + 30, player.MaxHP);
                            player.HealPotions--;
                            Console.WriteLine($"Ты выпил зелье лечения! HP теперь {player.HP}.");
                            used = true;
                        }
                        else if (choice == 8 && player.ManaPotions > 0)
                        {
                            player.Mana = Math.Min(player.Mana + 30, player.MaxMana);
                            player.ManaPotions--;
                            Console.WriteLine($"Ты выпил зелье маны! Мана теперь {player.Mana}.");
                            used = true;
                        }
                        else if (choice == 9 && player.ArmorPotions > 0)
                        {
                            player.ArmorCharges = 3;
                            player.ArmorPotions--;
                            Console.WriteLine("Ты выпил зелье стойкости! Теперь 3 заряда (барьер недоступен).");
                            used = true;
                        }
                        else if (choice == 0 && player.StrangePotions > 0)
                        {
                            int effect = rnd.Next(4);
                            player.StrangePotions--;
                            switch (effect)
                            {
                                case 0:
                                    player.HP = Math.Min(player.HP + 30, player.MaxHP);
                                    Console.WriteLine($"Странное зелье оказалось лечащим! HP теперь {player.HP}.");
                                    break;
                                case 1:
                                    player.Mana = Math.Min(player.Mana + 30, player.MaxMana);
                                    Console.WriteLine($"Странное зелье оказалось магическим! Мана теперь {player.Mana}.");
                                    break;
                                case 2:
                                    player.ArmorCharges = 3;
                                    Console.WriteLine("Странное зелье оказалось зельем стойкости! 3 заряда.");
                                    break;
                                case 3:
                                    player.HP -= 20;
                                    Console.WriteLine("О нет! Это ядовитая микстура! Вы теряете 20 HP.");
                                    break;
                            }
                            used = true;
                        }
                        else
                        {
                            Console.WriteLine("У тебя нет такого зелья!");
                            continue;
                        }

                        if (used)
                        {
                            turnUsed = true;
                            player.ResetBarrier();
                        }
                    }
                    else if (choice == 10)
                    {
                        Console.WriteLine("Ты погружаешься в медитацию, восстанавливая внутренний баланс...");
                        turnUsed = true;
                        player.ResetBarrier();
                    }
                    else
                    {
                        Console.WriteLine("Неверный ввод.");
                    }
                }

                if (boss.HP <= 0) return true;

                int dmg = boss.Attack(rnd);
                if (player.ArmorCharges > 0)
                {
                    player.ArmorCharges--;
                    Console.WriteLine($"Зелье стойкости поглощает урон! Осталось зарядов: {player.ArmorCharges}.");
                }
                else if (player.BarrierActive)
                {
                    Console.WriteLine("Колдун атакует, но барьер поглощает урон!");
                }
                else
                {
                    player.HP -= dmg;
                    Console.WriteLine($"Колдун атакует и наносит {dmg} урона!");
                }

                if (player.HP <= 0) return false;

                player.RegenerateMana(10); // ИЗМЕНЕНО с 20 на 10
                Console.WriteLine("Мана восстановлена на 10.");
                PressAnyKey();
            }
            return true;
        }

        static void DropPotion(Player player)
        {
            int roll = rnd.Next(100);
            if (roll < 30) // лечение
            {
                if (player.HealPotions < 2)
                {
                    player.HealPotions++;
                    Console.WriteLine("С голема выпало зелье лечения!");
                }
                else
                {
                    Console.WriteLine("С голема выпало зелье лечения, но у тебя уже два таких — ты не можешь нести больше.");
                }
            }
            else if (roll < 60) // мана
            {
                if (player.ManaPotions < 2)
                {
                    player.ManaPotions++;
                    Console.WriteLine("С голема выпало зелье маны!");
                }
                else
                {
                    Console.WriteLine("С голема выпало зелье маны, но у тебя уже два таких.");
                }
            }
            else if (roll < 80) // стойкость
            {
                if (player.ArmorPotions < 2)
                {
                    player.ArmorPotions++;
                    Console.WriteLine("С голема выпало зелье стойкости!");
                }
                else
                {
                    Console.WriteLine("С голема выпало зелье стойкости, но у тебя уже два таких.");
                }
            }
            else // странное
            {
                if (player.StrangePotions < 2)
                {
                    player.StrangePotions++;
                    Console.WriteLine("С голема выпало странное зелье!");
                }
                else
                {
                    Console.WriteLine("С голема выпало странное зелье, но у тебя уже два таких.");
                }
            }
        }
    }
}