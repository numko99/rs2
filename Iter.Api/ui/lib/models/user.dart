import 'package:json_annotation/json_annotation.dart';
import 'package:ui/models/agency.dart';
import 'package:ui/models/address.dart';

part 'user.g.dart';

@JsonSerializable()
class User {
  final String id;
  final String? firstName;
  final String? lastName;
  final String? email;
  final String? phoneNumber;
  final DateTime birthDate;
  final String birthPlace;
  final int role;
  final Address? address;
  final Agency? agency;
  final bool isActive;


  User({
    required this.id,
    required this.firstName,
    required this.lastName,
    required this.email,
    required this.phoneNumber,
    required this.birthDate,
    required this.birthPlace,
    required this.role,
    required this.address,
    required this.agency,
    required this.isActive
  });

    factory User.fromJson(Map<String, dynamic> json) => _$UserFromJson(json);

    Map<String, dynamic> toJson() => _$UserToJson(this);
}