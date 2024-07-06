import 'package:json_annotation/json_annotation.dart';
import 'package:ui/models/image_model.dart';

part 'reservation_search_respose.g.dart';

@JsonSerializable()
class ReservationSearchResponse {
  final String reservationId;
  final ImageModel mainImage;
  final String arrangementName;
  final DateTime arrangementStartDate;
  final DateTime? arrangementEndDateTime;
  final int reservationStatusId;
  final String reservationStatusName;
  final String agencyName;
  final double arrangementPrice;
  final double totalPaid;
  final DateTime reservationDate;
  final String firstName;
  final String lastName;
  final String reservationNumber;


  ReservationSearchResponse({
    required this.reservationId,
    required this.mainImage,
    required this.arrangementName,
    required this.arrangementStartDate,
    this.arrangementEndDateTime,
    required this.reservationStatusId,
    required this.reservationStatusName,
    required this.agencyName,
    required this.arrangementPrice,
    required this.totalPaid,
    required this.reservationDate,
    required this.firstName,
    required this.lastName,
    required this.reservationNumber,
  });

    factory ReservationSearchResponse.fromJson(Map<String, dynamic> json) =>
      _$ReservationSearchResponseFromJson(json);

  Map<String, dynamic> toJson() => _$ReservationSearchResponseToJson(this);
}