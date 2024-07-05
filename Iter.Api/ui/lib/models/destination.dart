import 'package:ui/models/accomodation.dart';
import 'package:json_annotation/json_annotation.dart';

part 'destination.g.dart';

@JsonSerializable()
class Destination {
  String? id;
  String? city;
  int? cityId;
  int? countryId;
  String? country;
  DateTime? arrivalDate;
  DateTime? departureDate;
  bool? isOneDayTrip;
  Accomodation? accommodation;

  Destination(
      {
      this.id,
      this.city,
      this.country,
      this.cityId,
      this.countryId,
      this.arrivalDate,
      this.departureDate,
      this.isOneDayTrip,
      this.accommodation});

  factory Destination.fromJson(Map<String, dynamic> json) =>
      _$DestinationFromJson(json);
  Map<String, dynamic> toJson() => _$DestinationToJson(this);
}
