import 'package:ui/models/accomodation.dart';
import 'package:json_annotation/json_annotation.dart';

part 'destination.g.dart';

@JsonSerializable()
class Destination {
  String? city;
  String? country;
  DateTime? arrivalDate;
  DateTime? departureDate;
  bool? isOneDayTrip;
  Accomodation? accommodation;

  Destination(
      {this.city,
      this.country,
      this.arrivalDate,
      this.departureDate,
      this.isOneDayTrip,
      this.accommodation});

  factory Destination.fromJson(Map<String, dynamic> json) =>
      _$DestinationFromJson(json);
  Map<String, dynamic> toJson() => _$DestinationToJson(this);
}
