// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'address.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Address _$AddressFromJson(Map<String, dynamic> json) => Address(
      id: json['id'] as String?,
      street: json['street'] as String?,
      houseNumber: json['houseNumber'] as String?,
      city: json['city'] as String?,
      postalCode: json['postalCode'] as String?,
      country: json['country'] as String?,
    );

Map<String, dynamic> _$AddressToJson(Address instance) => <String, dynamic>{
      'id': instance.id,
      'street': instance.street,
      'houseNumber': instance.houseNumber,
      'city': instance.city,
      'postalCode': instance.postalCode,
      'country': instance.country,
    };
