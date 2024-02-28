class ReservationStatusHelper {
   static String created = "Zaprimljena";
   static String paidIncomplete = "Plaćeno nepotpuno";
   static String painCompleted = "Plaćeno";
   static String canceled = "Otkazano";

  static String getStatusName(int role){
    if (role == 0)
    {
      return created;
    }
    else if (role == 1){
      return paidIncomplete;
    }
    else if (role == 2){
      return painCompleted;
    }
     else if (role == 3) {
      return canceled;
    }
    else{
      return "";
    }
  } 
}
