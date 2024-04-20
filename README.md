Серверная часть спроектирована как REST WEB API приложение. Для работы с БД на сервере используется ORM Entity Framework, также реализовано: логгирование для выявления ошибок, кэширование данных для более быстрой работы с бд, сервис по работе с галлереeй покрыт модульными тестами для обеспечения надёжности работы серверной части проекта. 

Клиентское WPF приложение предоставляет функционал для просмотра ранее отправленных данных в виде списка, где каждая запись содержит текст и соответствующее изображение. Приложение поддерживает возможность сохранения данных на сервере, чтобы пользователи могли получить доступ к ним при последующих сеансах, а также редактирование и удаление записей. Использует архитектуру MVVM, 


Навигация в приложении представлена вкладками: 
![image](https://github.com/ProgressiveKid/GalleryApp/assets/71317131/7edaa127-fbbf-4329-80ae-0c803cd4985d)
1) Форма добавления нового изображения в галерею:
![image](https://github.com/ProgressiveKid/GalleryApp/assets/71317131/1f87d0eb-88fc-4506-b7f8-95b98ed1f843)
![image](https://github.com/ProgressiveKid/GalleryApp/assets/71317131/fea1923b-0c11-481b-89f0-eb31fcc83639)
Сообщение об отправки на сервер
![image](https://github.com/ProgressiveKid/GalleryApp/assets/71317131/85e9baf6-407f-4efe-aa74-5718f89f4dd6)
Присутсвует валидация: на размер изображения и длину описания изображения
![image](https://github.com/ProgressiveKid/GalleryApp/assets/71317131/1a488387-7fe1-42d1-bb19-a402188bdf49)
![image](https://github.com/ProgressiveKid/GalleryApp/assets/71317131/41fe36ba-d3ac-4c3c-aea9-f436ca454345)
2) Форма просмотра всех изображении в галереии:
