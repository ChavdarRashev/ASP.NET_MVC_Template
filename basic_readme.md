# ������� ��������
- ��� ������������ �� ��� ������ �� �������� ���� �������� (template) � ������� ���������������.
- ��������� � �������� �� ������� �� MVC ��������� (patern) � ASP.NET Core 5 ������������ � EntityFramework Core 5
- ����� ���������� �����  ���� �� ���� �������� ���� ����������� �� ��������� ������ �� template �������� �� � ����� Data. �������� ������ �� BaseModel � BaseDeletableModel, ����� ���������� ������������ IBaseEntity � IDeletableEntity
- BaseModel � ���������� ���� � ��������� �������� (properties), ����� ��������� ����� ����� ������� � MVC patern - Id, CreatedOn, ModifiedOn 
- BaseDeletableModel � ���������� ���� � ������� ������� �� ����������� �� Soft Delete �������. ������� �� �� ����� , � ���� �� ��������, ���� ������� � �� ����� ���� �� ���������.
- �� ����������� �� Soft Delete �� ��������� Query filters ������ �� EntityFramework Core ������������ ��� ����������� �� ������������. �� ����� �����, ����� � "soft deleted" � DBContext ����� �� �������� Query filter.
- �� �������� ���� soft deleted �� �� override ���  SaveChanges Remove �������� �� EntityFramework. �� ������������� �� Soft Delete �� �������� ��� ����� RemoveSoftAsync � DBContext-a