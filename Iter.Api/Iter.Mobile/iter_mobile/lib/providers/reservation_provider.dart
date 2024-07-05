import 'dart:convert';

import 'package:iter_mobile/models/reservation.dart';
import 'package:iter_mobile/models/reservation_search_respose.dart';
import 'base_provider.dart';

class ReservationProvider
    extends BaseProvider<Reservation, ReservationSearchResponse> {
  ReservationProvider() : super("Reservation");

  @override
  Reservation fromJson(data) {
    return Reservation.fromJson(data);
  }

  @override
  ReservationSearchResponse fromJsonSearch(data) {
    return ReservationSearchResponse.fromJson(data);
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

  Future<void> addReview(var reservationId, int rating) async {
    var url = Uri.parse("$baseUrl$endpoint/addReview");

    Map<String, String>? headers = await createHeaders();
    Map<String, dynamic> request = {
      'reservationId': reservationId,
      'rating': rating
    };

    await http!.post(
      url,
      headers: headers,
      body: jsonEncode(request),
    );
  }

    Future<void> addPayment(var reservationId, double? totalPaid, String transactionId) async {
    var url = Uri.parse("$baseUrl$endpoint/addPayment");

    Map<String, String>? headers = await createHeaders();
    Map<String, dynamic> request = {
      'reservationId': reservationId,
      'totalPaid': totalPaid?.round(),
      'transactionId': transactionId,
    };

    await http!.post(
      url,
      headers: headers,
      body: jsonEncode(request),
    );
  }

  Future<void> cancelReservation(var reservationId) async {
    var url = Uri.parse("$baseUrl$endpoint/cancelReservation/$reservationId");

    Map<String, String>? headers = await createHeaders();

    await http!.get(
      url,
      headers: headers,
    );
  }
}
