import 'package:json_annotation/json_annotation.dart';
import 'package:ui/models/agency.dart';
import 'package:ui/models/address.dart';
import 'package:ui/models/arrangement_price.dart';
import 'package:ui/models/arrangment.dart';
import 'package:ui/models/user.dart';

part 'reservation.g.dart';

@JsonSerializable()
class Reservation {
  final String id;
  final String? arrangementPriceId;
  final String? reservationNumber;
  final String? departurePlace;
  final int departureCityId;
  final User? user;
  final Arrangement? arrangement;
  final ArrangementPrice? arrangementPrice;
  final int? reservationStatusId;
  final String? reservationStatusName;
  final double? totalPaid;
  final int? rating;
  final String? reminder;

  Reservation(
      {
      required this.id,
      required this.reservationNumber,
      required this.arrangementPriceId,
      required this.departurePlace,
      required this.departureCityId,
      required this.user,
      required this.arrangement,
      required this.arrangementPrice,
      required this.reservationStatusId,
      required this.reservationStatusName,
      required this.totalPaid,
      required this.rating,
      required this.reminder
      });

  factory Reservation.fromJson(Map<String, dynamic> json) => _$ReservationFromJson(json);

  Map<String, dynamic> toJson() => _$ReservationToJson(this);
}
