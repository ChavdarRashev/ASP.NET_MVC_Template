# Основни принципи
- При изграждането на нов проект се използва този темплейт (template) с основни функционалности.
- Темплейта е построен по приците на MVC обраазеца (patern) с ASP.NET Core 5 технологията и EntityFramework Core 5
- всеки произволен модел  може да бъде създаден чрез наследяване на основните обекти от template намиращи се в папка Data. Осовните обекти са BaseModel и BaseDeletableModel, които наследяват интерфейсите IBaseEntity и IDeletableEntity
- BaseModel е абстрактен клас с основните свойства (properties), които притежава всеки модел свързан с MVC patern - Id, CreatedOn, ModifiedOn 
- BaseDeletableModel е абстрактен клас с основни свойсва за реализиране на Soft Delete принцип. Данните не се трият , а само се маркират, като изтрити и се слага дата на изтриване.
- За реализиране на Soft Delete се използват Query filters заявки от EntityFramework Core технологията при изтеглянето на информацията. За всеки модел, който е "soft deleted" в DBContext класа се дефинира Query filter.
- За изтриане прио soft deleted не се override или  SaveChanges Remove методите на EntityFramework. За реализирането на Soft Delete се дефинира нов метод RemoveSoftAsync в DBContext-a