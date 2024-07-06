// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'accomodation.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Accomodation _$AccomodationFromJson(Map<String, dynamic> json) => Accomodation(
      hotelName: json['hotelName'] as String?,
      checkInDate: json['checkInDate'] == null
          ? null
          : DateTime.parse(json['checkInDate'] as String),
      checkOutDate: json['checkOutDate'] == null
          ? null
          : DateTime.parse(json['checkOutDate'] as String),
      hotelAddress: json['hotelAddress'] == null
          ? null
          : Address.fromJson(json['hotelAddress'] as Map<String, dynamic>),
    );

Map<String, dynamic> _$AccomodationToJson(Accomodation instance) =>
    <String, dynamic>{
      'hotelName': instance.hotelName,
      'checkInDate': instance.checkInDate?.toIso8601String(),
      'checkOutDate': instance.checkOutDate?.toIso8601String(),
      'hotelAddress': instance.hotelAddress,
    };
