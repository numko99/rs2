import 'package:intl/intl.dart';

class Filter {
  DateTime? startDate;
  DateTime? endDate;
  double? rating;

  Filter({this.startDate, this.endDate, this.rating});

  bool get hasFilters => startDate != null || endDate != null || rating != null;

  List<Map<String, String>> get activeFilters {
    List<Map<String, String>> active = [];

    if (startDate != null && endDate != null) {
      active.add({
        'date':
            "${DateFormat('dd.MM.yyyy').format(startDate!)}-${DateFormat('dd.MM.yyyy').format(endDate!)}"
      });
    }

    if (rating != null) {
      active.add({'rating': rating!.toStringAsPrecision(1)});
    }
    return active;
  }

  void clear() {
    startDate = null;
    endDate = null;
    rating = null;
  }
}
