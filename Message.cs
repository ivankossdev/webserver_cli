namespace webserver_cli;

public static class Message
{
    public static readonly string start = "Программа запущена.";
    public static readonly string enterNumPort = "Выбирете номер порта: ";
    public static readonly string exit = "\'Esc\' выход из программы.";
    public static readonly string searchedDevice = "Найденные устройства:";
    public static readonly string errorNumPort = " Неверный номер порта.";
    public static readonly string pointsMenu = "Синхронизировать: \n" +
                                               "[ 1 ] Время \n" + 
                                               "[ 2 ] Дату \n" + 
                                               "[ 3 ] День недели \n" + 
                                               "[ 4 ] Месяц\n" + 
                                               "[ 5 ] Год\n";
    public static readonly string ready = "Готово"; 
}