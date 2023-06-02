# CallappProjectRepo

დავალების ორივე ნაწილი შესრულებულია.
დავალების შესასრულებლად გამოვიყენე შემდეგი ტექნოლოგიები: .NET 7, ASP.NET CORE(MVC),ASP.NET CORE(WEB API),Class Library, EF Core, JWT for API User authentication and Application User Authentication.
Swagger_ში აუტორიზაციის გასავლელად დარეგისტრირდით და შედით(API Authentication controller endpointe_ები სვაგერის აუთენთიფიკაციას არ საჭიროებს) როგორ API User, Login_ის რესფონსად თქვენ მიიღებთ JWT რომელიც საშუალებას მოგცემთ swagger_ის აუტენთიფიკაციის გავლისა('bearer YourJWT').

ვინაიდან არ იყო დავალებაში მითითებული თუ კონკრეტულად რომელი Data Accesss Layer(EF Core, Dapper, ADO.NET) უნდა გამოგვეყენებინა, მე გამოვიყენე EF Core და  Code First approach_ი.
ვინაიდან ეს პროექტი Github_ზე უნდა აგვეტვირთა application secrets(JWT For MVC project, JWT Secret Key For WEB API project) არ შევინახე როგორც user secrets(რაც უფრო დაცულს გახდიდა საიდუმლოებეს და სოურს კონტროლით არ გამოააშკარავებდა) და ისინი შენახულია appsettings.json ფაილში როგორც MVC ასევე WEB API პროექტში.

MVC და WEB API პროექტებთან ასევე გამოყენებულია Class Library_ რომელიც ინახავს წინად ხსენებული პროექტების საერთო კოდს. შესაძლებელი იყო უბრალოდ WEB API პროექტი დამერეფერენსებინა MVC პროექტთან თუცა პირადი გამოცდილებიდან გამომდინარე ვიცი რომ მსგავსი მიდგომა ართულებს აპლიკაციის დაჰოსტვას(ჩემთვის კონკრეტულად ეს პრობლემა წარმოიშვა Microsoft Azur_ზე) სწორედ ამიტომა გამოყენებული ეგრედწოდებული Shared Library რომელიც სრულიად იცილებს თავიდან პროექტების ერთმანეთზე დამოკიდებულებას რაც ასევე არის ერთ-ერთი მთავარი იდეა კონცებციისა Separation Of Concerns.

გამოყენებული დიზაინ პატერნები: Dependency Injection, Repository Pattern.
Entity Relations დაკონფიგურირებულია Fluent API_ს საშუალებით.
