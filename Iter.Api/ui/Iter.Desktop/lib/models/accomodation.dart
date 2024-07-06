import 'package:ui/models/address.dart';

import 'package:json_annotation/json_annotation.dart';

part 'accomodation.g.dart';

@JsonSerializable()
class Accomodation {
  String? hotelName;
  DateTime? checkInDate;
  DateTime? checkOutDate;
  Address? hotelAddress;

  Accomodation(
      {this.hotelName, this.checkInDate, this.checkOutDate, this.hotelAddress});

  factory Accomodation.fromJson(Map<String, dynamic> json) =>
      _$AccomodationFromJson(json);
  Map<String, dynamic> toJson() => _$AccomodationToJson(this);
}
