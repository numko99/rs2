import 'dart:convert';

import 'package:ui/models/reservation.dart';
import 'base_provider.dart';

class ReservationProvider extends BaseProvider<Reservation> {
  ReservationProvider() : super("Reservation");

  @override
  Reservation fromJson(data) {
    return Reservation.fromJson(data);
  }

   Future<int> getCount(var arrangementId) async {
    var url = Uri.parse("$baseUrl$endpoint/count/$arrangementId");

    Map<String, String>? headers = await createHeaders();

    var response = await http!.get(url, headers: headers);
    if (isValidResponseCode(response)) {
      var data = jsonDecode(response.body);
      return data;
    } else {
      throw Exception("Exception... handle this gracefully");
    }
  }
}
