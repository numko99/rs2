import 'package:json_annotation/json_annotation.dart';

part 'user_statistic_data.g.dart';

@JsonSerializable()

class UserStatisticData {
  int? reservationCount;
  double? avgRating;
  int arrangementsCount;
  String? firstName;
  String? lastName;

  UserStatisticData({
    this.reservationCount,
    required this.arrangementsCount,
    this.avgRating,
    this.firstName,
    this.lastName,
  });

  factory UserStatisticData.fromJson(Map<String, dynamic> json) => _$UserStatisticDataFromJson(json);

  Map<String, dynamic> toJson() => _$UserStatisticDataToJson(this);
}
