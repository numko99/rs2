import 'package:json_annotation/json_annotation.dart';

part 'address.g.dart';

@JsonSerializable()
class Address {
  final String? id;
  final String? street;
  final String? houseNumber;
  final String? city;
  final String? cityId;
  final String? postalCode;
  final String? country;

  Address({
    this.id,
    required this.street,
    required this.houseNumber,
    this.city,
    required this.cityId,
    required this.postalCode,
    this.country,
  });

  factory Address.fromJson(Map<String, dynamic> json) => _$AddressFromJson(json);

  Map<String, dynamic> toJson() => _$AddressToJson(this);
}