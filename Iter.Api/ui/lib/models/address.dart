import 'package:json_annotation/json_annotation.dart';

part 'address.g.dart';

@JsonSerializable()
class Address {
  final String? id;
  final String street;
  final String houseNumber;
  final String city;
  final String postalCode;
  final String country;

  Address({
    this.id,
    required this.street,
    required this.houseNumber,
    required this.city,
    required this.postalCode,
    required this.country,
  });

  factory Address.fromJson(Map<String, dynamic> json) => _$AddressFromJson(json);

  Map<String, dynamic> toJson() => _$AddressToJson(this);
}