import 'package:json_annotation/json_annotation.dart';

part 'user_payment.g.dart';

@JsonSerializable()

class UserPaymentModel {
  String? firstName;
  String? lastName;
  double? totalPaid;
  String? reservationNumber;
  String? arrangementId;
  String? arrangementName;
  String? transactionId;

  UserPaymentModel({
    this.firstName,
    this.lastName,
    this.totalPaid,
    this.reservationNumber,
    this.arrangementId,
    this.arrangementName,
    this.transactionId,
  });

      factory UserPaymentModel.fromJson(Map<String, dynamic> json) =>
      _$UserPaymentModelFromJson(json);

  Map<String, dynamic> toJson() => _$UserPaymentModelToJson(this);
}