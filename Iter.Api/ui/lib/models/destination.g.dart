// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'destination.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Destination _$DestinationFromJson(Map<String, dynamic> json) => Destination(
      city: json['city'] as String?,
      country: json['country'] as String?,
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
      'city': instance.city,
      'country': instance.country,
      'arrivalDate': instance.arrivalDate?.toIso8601String(),
      'departureDate': instance.departureDate?.toIso8601String(),
      'isOneDayTrip': instance.isOneDayTrip,
      'accommodation': instance.accommodation,
    };
