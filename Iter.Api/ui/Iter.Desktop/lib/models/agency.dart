import 'package:json_annotation/json_annotation.dart';
import 'package:ui/models/image_model.dart';
import 'address.dart';

part 'agency.g.dart';

@JsonSerializable()
class Agency {
  final String id;
  final String name;
  final Address? address;
  final String contactEmail;
  final String contactPhone;
  final String website;
  final String licenseNumber;
  final ImageModel? logo;
  final bool isActive;
  final double rating;
  final bool isDeleted;
  final DateTime dateCreated;
  final DateTime dateModified;

  Agency({
    required this.id,
    required this.name,
    this.address,
    required this.contactEmail,
    required this.contactPhone,
    required this.website,
    required this.licenseNumber,
    required this.isActive,
    required this.logo,
    required this.rating,
    required this.isDeleted,
    required this.dateCreated,
    required this.dateModified,
  });

  factory Agency.fromJson(Map<String, dynamic> json) => _$AgencyFromJson(json);

  Map<String, dynamic> toJson() => _$AgencyToJson(this);
}