// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'agency.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Agency _$AgencyFromJson(Map<String, dynamic> json) => Agency(
      id: json['id'] as String,
      name: json['name'] as String,
      address: json['address'] == null
          ? null
          : Address.fromJson(json['address'] as Map<String, dynamic>),
      contactEmail: json['contactEmail'] as String,
      contactPhone: json['contactPhone'] as String,
      website: json['website'] as String,
      licenseNumber: json['licenseNumber'] as String,
      isActive: json['isActive'] as bool,
      logo: json['logo'] == null
          ? null
          : ImageModel.fromJson(json['logo'] as Map<String, dynamic>),
      rating: (json['rating'] as num).toDouble(),
      isDeleted: json['isDeleted'] as bool,
      dateCreated: DateTime.parse(json['dateCreated'] as String),
      dateModified: DateTime.parse(json['dateModified'] as String),
    );

Map<String, dynamic> _$AgencyToJson(Agency instance) => <String, dynamic>{
      'id': instance.id,
      'name': instance.name,
      'address': instance.address,
      'contactEmail': instance.contactEmail,
      'contactPhone': instance.contactPhone,
      'website': instance.website,
      'licenseNumber': instance.licenseNumber,
      'logo': instance.logo,
      'isActive': instance.isActive,
      'rating': instance.rating,
      'isDeleted': instance.isDeleted,
      'dateCreated': instance.dateCreated.toIso8601String(),
      'dateModified': instance.dateModified.toIso8601String(),
    };
