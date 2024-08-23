// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'destination.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Destination _$DestinationFromJson(Map<String, dynamic> json) => Destination(
      id: json['id'] as String?,
      city: json['city'] as String?,
      country: json['country'] as String?,
      cityId: (json['cityId'] as num?)?.toInt(),
      countryId: (json['countryId'] as num?)?.toInt(),
      arrivalDate: json['arrivalDate'] == null
          ? null
          : DateTime.parse(json['arrivalDate'] as String),
      departureDate: json['departureDate'] == null
          ? null
          : DateTime.parse(json['departureDate'] as String),
      isOneDayTrip: json['isOneDayTrip'] as bool?,
      accommodation: json['accommodation'] == null
          ? null
          : Accomodation.fromJson(
              json['accommodation'] as Map<String, dynamic>),
    );

Map<String, dynamic> _$DestinationToJson(Destination instance) =>
    <String, dynamic>{
      'id': instance.id,
      'city': instance.city,
      'cityId': instance.cityId,
      'countryId': instance.countryId,
      'country': instance.country,
      'arrivalDate': instance.arrivalDate?.toIso8601String(),
      'departureDate': instance.departureDate?.toIso8601String(),
      'isOneDayTrip': instance.isOneDayTrip,
      'accommodation': instance.accommodation,
    };
