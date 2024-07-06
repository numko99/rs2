class RoleHelper {
   static String coordinator = "Koordinator";
   static String employee = "Vodič";
   static String client = "Klijent";

  static String GetRoleName(int role){
    if (role == 2)
    {
      return coordinator;
    }
    else if (role == 3){
      return employee;
    }
    else if (role == 4){
      return client;
    }
    else{
      return "";
    }
  } 
}
