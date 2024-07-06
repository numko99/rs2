// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'user_statistic_data.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

UserStatisticData _$UserStatisticDataFromJson(Map<String, dynamic> json) =>
    UserStatisticData(
      reservationCount: json['reservationCount'] as int?,
      arrangementsCount: json['arrangementsCount'] as int,
      avgRating: (json['avgRating'] as num?)?.toDouble(),
      firstName: json['firstName'] as String?,
      lastName: json['lastName'] as String?,
    );

Map<String, dynamic> _$UserStatisticDataToJson(UserStatisticData instance) =>
    <String, dynamic>{
      'reservationCount': instance.reservationCount,
      'avgRating': instance.avgRating,
      'arrangementsCount': instance.arrangementsCount,
      'firstName': instance.firstName,
      'lastName': instance.lastName,
    };
