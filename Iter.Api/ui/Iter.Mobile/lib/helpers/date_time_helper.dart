class DateTimeCustomHelper {
  static String getTimeSince(DateTime date) {
    Duration difference = DateTime.now().difference(date);

    if (difference.inDays > 0) {
      return '${difference.inDays} dan${difference.inDays > 1 ? 'a' : ''}';
    } else if (difference.inHours > 0) {
      return '${difference.inHours} sat${difference.inHours > 1 ? 'i' : ''}';
    } else if (difference.inMinutes > 0) {
      return '${difference.inMinutes} minut${difference.inMinutes > 1 ? 'a' : ''}';
    } else if (difference.inSeconds > 0) {
      return '${difference.inSeconds} sekund${difference.inSeconds > 1 ? 'i' : ''}';
    } else {
      return 'upravo sada';
    }
  }

    static bool isBeforeDateOnly(DateTime date1, DateTime date2) {
    DateTime simplifiedDate1 = DateTime(date1.year, date1.month, date1.day);
    DateTime simplifiedDate2 = DateTime(date2.year, date2.month, date2.day);

    return simplifiedDate1.isBefore(simplifiedDate2);
  }

  static bool isAfterDateOnly(DateTime date1, DateTime date2) {
    DateTime simplifiedDate1 = DateTime(date1.year, date1.month, date1.day);
    DateTime simplifiedDate2 = DateTime(date2.year, date2.month, date2.day);

    return simplifiedDate1.isAfter(simplifiedDate2);
  }

    static bool areDatesEqual(DateTime date1, DateTime date2) {
    var a = date1.year == date2.year &&
        date1.month == date2.month &&
        date1.day == date2.day;

        return a;
  }

  static bool isAfterOrEqualDateOnly(DateTime date1, DateTime date2) {
    var after = isAfterDateOnly(date1, date2);
    var equal = areDatesEqual(date1, date2);

    return after || equal;
  }

    static bool isBeforeOrEqualDateOnly(DateTime date1, DateTime date2) {
    var after = isBeforeDateOnly(date1, date2);
    var equal = areDatesEqual(date1, date2);

    return after || equal;
  }

}
