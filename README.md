Подробни инструкции с препоръки за скрийншоти
1. Инсталиране на Visual Studio 2022
•	Отиди на: https://visualstudio.microsoft.com/
•	Изтегли 'Visual Studio 2022 Community Edition'.
![image](https://github.com/user-attachments/assets/2f247d3e-3f92-4f22-b841-fbe3f681873f)

•	Стартирай инсталацията.
•	На екрана с 'Workloads', избери следното:
•	 - ASP.NET and web development
•	 - .NET Desktop Development
•	 - SQL Server Data Tools (ако не е маркиран)
•	Натисни бутона 'Install' и изчакай инсталацията да приключи.
![image](https://github.com/user-attachments/assets/21fe50d6-a55d-4e71-a714-0f741735e69c)

 2. Инсталиране на SSMS (SQL Server Management Studio)
•	Отиди на: https://aka.ms/ssmsfullsetup
•	Изтегли SSMS и стартирай инсталацията.
•	Следвай инструкциите и избери инсталация по подразбиране.
•	След края на инсталацията, рестартирай компютъра (ако е необходимо).
  ![image](https://github.com/user-attachments/assets/325e32b1-2bda-4a16-a8de-03b408793b97)

3. Изтегляне на проекта от GitHub
•	Увери се, че имаш инсталиран Git (https://git-scm.com/downloads).
•	Създай папка, например C:\Projects.
•	Отвори Command Prompt или Git Bash.
•	Напиши следната команда:
•	   git clone https://github.com/FilipNedelchev20/OnlineShopForFitnessEquipment.git
•	Изчакай да се изтегли проектът.
![image](https://github.com/user-attachments/assets/9a9d6e7f-da94-462b-aba6-844ebfb77687)
![image](https://github.com/user-attachments/assets/97fa873c-2624-4eb7-a996-e61abbe4c361)


    4. Отваряне на проекта с Visual Studio
•	Стартирай Visual Studio.
•	 ![image](https://github.com/user-attachments/assets/1091ffb4-d7be-4475-9aaf-e4e72e29cbad)

•	Избери 'Open a project or solution'.
•	Навигирай до мястото, където си клонирал проекта.
•	Избери файла OnlineShopForFitnessEquipment.sln и го отвори.
![image](https://github.com/user-attachments/assets/f73d42aa-328d-482d-9dc9-a98efad1a026)

  5. Конфигуриране на връзката към базата данни
•	Отвори файла appsettings.json в проекта.
•	Провери дали connection string е правилен, напр.:
•	   Server=localhost\SQLEXPRESS;Database=OnlineShopDB;Trusted_Connection=True;
•	Ако използваш SQL Authentication, добави User Id и Password.
![image](https://github.com/user-attachments/assets/92c4ddfd-537b-4b4a-ab2e-faebc77b73eb)

  6. Създаване на базата данни с Entity Framework
•	Отвори Package Manager Console от Visual Studio:
•	   Tools > NuGet Package Manager > Package Manager Console
•	 ![image](https://github.com/user-attachments/assets/fb49b30e-d53c-454b-89a4-5166e3925302)

•	Напиши командата: Update-Database и натисни Enter.
•	 ![image](https://github.com/user-attachments/assets/832a446a-8de6-4d1f-b010-bcfde5aa8d2e)

•	Изчакай създаването на базата данни.
7. Стартиране на проекта
•	Увери се, че главният проект е избран като 'StartUp Project'.
•	Натисни бутона Start (зелена стрелка) или клавиша F5.
•	 ![image](https://github.com/user-attachments/assets/db9ec2ad-c1c0-4a37-a4e3-71bc2a885e89)

•	Проектът ще се отвори в браузър.
 ![image](https://github.com/user-attachments/assets/fddee1e9-f704-41fb-b532-3443cf8e4626)

# -
